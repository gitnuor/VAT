using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.User;
using vms.repository.Repository.tbl;
using vms.service.Services.SecurityService;
using vms.entity.Utility;
using vms.entity.Dto.User;
using AutoMapper;
using URF.Core.Abstractions;
using vms.service.Services.SettingService;
using Microsoft.AspNetCore.DataProtection;

namespace vms.service.ServiceImplementations.SecurityService;

public class UserService : ServiceBase<User>, IUserService
{
	private readonly IUserRepository _repository;
	private readonly IUserBranchService _userBranchService;
	private readonly IIntegratedApplicationService _integratedApplicationService;
	private readonly IIntegratedApplicationRefDataService _integratedApplicationRefDataService;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRoleService _roleService;
	private IDataProtector _dataProtector;

	public UserService(IUserRepository repository, IUserBranchService userBranchService,
		IIntegratedApplicationService integratedApplicationService, IUnitOfWork unitOfWork,
		IIntegratedApplicationRefDataService integratedApplicationRefDataService, IRoleService roleService, IMapper mapper, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(repository)
	{
		_repository = repository;
		_userBranchService = userBranchService;
		_integratedApplicationService = integratedApplicationService;
		_integratedApplicationRefDataService = integratedApplicationRefDataService;
		_unitOfWork = unitOfWork;
		_roleService = roleService;
		_mapper = mapper;
		_dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
	}

	public async Task<IEnumerable<User>> GetUsers(int orgIdEnc)
	{
		var users = (await _repository.GetUsers(orgIdEnc)).ToList();
		users.ForEach(u => u.EncryptedId = _dataProtector.Protect(u.UserId.ToString()));
		return users;
	}

	public async Task<IEnumerable<ViewUser>> GetAllByOrganization(string encOrganizationId)
	{
		var users = (await _repository.GetAllUserByOrganization(encOrganizationId)).ToList();
		users.ForEach(u => u.EncryptedId = _dataProtector.Protect(u.UserId.ToString()));
		return users;
	}

	public async Task<IEnumerable<User>> GetAllByOrganization(string encOrganizationId, EnumUserStatus userStatus)
	{
		var users = await _repository.GetAllByOrganization(encOrganizationId);
		switch (userStatus)
		{
			case EnumUserStatus.Active:
				users = users.Where(u => u.IsActive);
				break;
			case EnumUserStatus.Inactive:
				users = users.Where(u => !u.IsActive);
				break;
			case EnumUserStatus.All:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(userStatus), userStatus, null);
		}

		return users;
	}

	public async Task<IEnumerable<User>> GetAllActiveByOrganization(string encOrganizationId)
	{
		var users = await _repository.GetAllByOrganization(encOrganizationId);
		return users.Where(u => u.IsActive);
	}

	public async Task<IEnumerable<User>> GetAllInactiveByOrganization(string encOrganizationId)
	{
		var users = await _repository.GetAllByOrganization(encOrganizationId);
		return users.Where(u => !u.IsActive);
	}

	public async Task<User> GetUser(string idEnc)
	{
		return await _repository.GetUser(idEnc);
	}

	public async Task<bool> IsUserExists(string userName)
	{
		return await _repository.Query().AnyAsync(u => u.UserName == userName, CancellationToken.None);
	}

	public Task<User> GetUserByUsername(string userName)
	{
		return _repository.GetUserByUsername(userName);
	}

	public Task<User> GetUserByUsernameAndEncPass(string userName, string encPassword)
	{
		return _repository.GetUserByUsernameAndEncPass(userName, encPassword);
	}

	public async Task AddFailedCount(int id)
	{
		var user = await Query().FirstOrDefaultAsync(u => u.UserId == id);
		if (user != null)
		{
			user.AccessFailedCount = user.AccessFailedCount == null ? 0 : user.AccessFailedCount + 1;
			Update(user);
		}
	}

	public async Task AddFailedCountAndLockUser(int id)
	{
		var user = await Query().FirstOrDefaultAsync(u => u.UserId == id);
		if (user != null)
		{
			var failCount = user.AccessFailedCount == null ? 0 : user.AccessFailedCount + 1;
			user.AccessFailedCount = failCount;
			if (failCount >= 5)
			{
				user.IsLocked = true;
				user.LastLockReason = "Excess fail count.";
				user.LastLockTime = DateTime.Now;
			}

			Update(user);
		}
	}

	public async Task ResetFailedCount(int id)
	{
		var user = await Query().FirstOrDefaultAsync(u => u.UserId == id);
		if (user != null)
		{
			user.AccessFailedCount = 0;
			Update(user);
		}
	}

	public async Task ResetFailedCountAndUpdateLoginTime(int id)
	{
		var user = await Query().FirstOrDefaultAsync(u => u.UserId == id);
		if (user != null)
		{
			user.AccessFailedCount = 0;
			user.LastLoginTime = DateTime.Now;
			Update(user);
		}
	}

	public async Task UpdateLoginTime(int id)
	{
		var user = await Query().FirstOrDefaultAsync(u => u.UserId == id);
		if (user != null)
		{
			user.LastLoginTime = DateTime.Now;
			Update(user);
		}
	}

	public async Task InactivateUser(int id, int inactivateByUserId, string reason)
	{
		var user = await Query().FirstOrDefaultAsync(u => u.UserId == id);
		if (user != null)
		{
			user.IsActive = false;
			user.InActivationTime = DateTime.Now;
			user.InActivatedBy = inactivateByUserId;
			user.InActivationReason = reason;
			Update(user);
		}
	}

	public async Task LockUser(int id, string reason)
	{
		var user = await Query().FirstOrDefaultAsync(u => u.UserId == id);
		if (user != null)
		{
			user.IsLocked = true;
			user.LastLockTime = DateTime.Now;
			user.LastLockReason = reason;
			Update(user);
		}
	}

	public async Task<User> GetUserByOrgAndId(int id, int orgId)
	{
		return await Query().FirstOrDefaultAsync(u => u.UserId == id && u.OrganizationId == orgId);
	}

	public async Task<IEnumerable<CustomSelectListItem>> GetAllByOrganizationSelectList(string pOrgId)
	{
		//return new SelectList(await _repository.GetAllByOrganization(pOrgId), nameof(User.UserId), nameof(User.FullName));
		var users = await _repository.GetAllByOrganization(pOrgId);

		return users.ConvertToCustomSelectList(nameof(User.UserId),
			nameof(User.FullName));
	}

	public Task InsertUserBranches(IEnumerable<UserBranchCreateViewModel> userBranches, int orgId, int userId)
	{
		var branches = GetUserBranchesToSave(userBranches, orgId, userId);
		return branches.Any() ? _userBranchService.BulkInsertAsync(branches) : Task.CompletedTask;
	}

	#region private method

	private List<UserBranch> GetUserBranchesToSave(IEnumerable<UserBranchCreateViewModel> userBranches, int orgId,
		int userId)
	{
		return userBranches.Where(b => b.IsSelected)
			.Select(branchCreateViewModel => new UserBranch
			{
				UserId = userId,
				OrgBranchId = branchCreateViewModel.OrgBranchId,
				OrganizationId = orgId,
				IsActive = true,
				ExpiryDate = DateTime.Now,
				CreatedBy = userId,
				CreatedTime = DateTime.Now,
				ModifiedBy = userId,
				ModifiedTime = DateTime.Now,
			})
			.ToList();
	}

	#endregion


	#region Api

	public async Task InsertUserDataFromApi(UserPostDto userPostDto, string appKey, string userPassword)
	{
		var integratedApp = await _integratedApplicationService.GetIntegratedApplicationByAppKey(appKey);
		if (integratedApp == null)
			throw new Exception("Integrated application not found!");

		if (await _integratedApplicationRefDataService.IsReferenceDataExist(integratedApp.IntegratedApplicationId,
			    EnumReferenceDataType.User, userPostDto.Id))
			throw new Exception("Data already exist for this id.!");

		var userData = _mapper.Map<UserPostDto, User>(userPostDto);
		userData.IsActive = true;
		userData.OrganizationId = integratedApp.OrganizationId;
		userData.CreatedBy = integratedApp.CreatedBy;
		userData.CreatedTime = DateTime.Now;
		userData.UserTypeId = (int)EnumUserTypeOptions.Company;
		userData.Password = userPassword;

		#region user role check

		var roles = await _roleService.GetRoleByName(integratedApp.OrganizationId, userPostDto.RoleName);
		if (roles == null)
		{
			Role role = new Role();
			role.RoleName = userPostDto.RoleName;
			role.OrganizationId = userData.OrganizationId;
			role.CreatedBy = null;
			role.CreatedTime = DateTime.Now;
			role.IsActive = true;
			_roleService.Insert(role);
			await _unitOfWork.SaveChangesAsync();
			userData.RoleId = role.RoleId;
		}
		else
		{
			userData.RoleId = roles.RoleId;
		}

		#endregion

		await _repository.CustomInsertAsync(userData);
		await _unitOfWork.SaveChangesAsync();
		await _integratedApplicationRefDataService.InsertReferenceData(EnumReferenceDataType.User,
			integratedApp.IntegratedApplicationId, userData.UserId, userPostDto.Id);
	}

	public async Task<IEnumerable<UserDto>> GetAllForApi(string encOrganizationId)
	{
		var users = await _repository.GetAllUserByOrganization(encOrganizationId);
		return _mapper.Map<IEnumerable<ViewUser>, IEnumerable<UserDto>>(users);
	}

	#endregion
}