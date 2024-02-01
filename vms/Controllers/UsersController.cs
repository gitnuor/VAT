using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.entity.viewModels;
using vms.entity.viewModels.User;
using vms.service.Services.SecurityService;
using vms.service.Services.SettingService;
using vms.service.Services.UploadService;
using vms.utility;
using vms.utility.StaticData;
using vms.Utility;

namespace vms.Controllers;

public class UsersController : ControllerBase
{
	private readonly IUserService _service;
	private readonly IRoleService _roleService;
	private readonly IUserTypeService _userTypeService;
	private readonly IConfiguration _configuration;
	private readonly IMapper _mapper;
	private readonly IOrgBranchService _branchService;

	private readonly IFileOperationService _fileOperationService;

	public UsersController(ControllerBaseParamModel controllerBaseParamModel, IUserService service,
		IRoleService roleService, IUserTypeService userTypeService, IFileOperationService fileOperationService,
		IMapper mapper, IOrgBranchService branchService) : base(controllerBaseParamModel)
	{
		_service = service;
		_roleService = roleService;
		_userTypeService = userTypeService;
		_fileOperationService = fileOperationService;
		_mapper = mapper;
		_configuration = Configuration;
		_branchService = branchService;
	}

	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
	public async Task<IActionResult> Index()
	{
		return View(await _service.GetAllByOrganization(UserSession.ProtectedOrganizationId));
	}

	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW_DETAILS)]
	public async Task<IActionResult> Details(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return NotFound();
		}

		var user = await _service.GetUser(id);

		if (user == null)
		{
			return NotFound();
		}

		return View(user);
	}

	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_ADD)]
	public async Task<IActionResult> Create()
	{
		if (UserSession.OrganizationId == -1)
		{
			ViewData[ViewStaticData.UserType] = EnumUserTypeOptions.Master;
		}
		else
		{
			ViewData[ViewStaticData.UserType] = EnumUserTypeOptions.Company;
		}

		var roleDt = await _roleService.Query().SelectAsync();
		var roles = roleDt.Where(c => c.OrganizationId == UserSession.OrganizationId).Select(s => new CustomSelectListItem
		{
			Id = s.RoleId,
			Name = s.RoleName
		});

		var userTypeDt = await _userTypeService.Query().SelectAsync();
		var userTypes = userTypeDt.Select(s => new CustomSelectListItem
		{
			Id = s.UserTypeId,
			Name = s.UserTypeName
		});

		var user = new UserCreateViewModel
		{
			Roles = roles,
			UserTypes = userTypes
		};

		var branches = await _branchService.GetOrgBranchByOrganization(UserSession.ProtectedOrganizationId);

		foreach (var branch in branches)
		{
			user.UserBranchList.Add(new UserBranchCreateViewModel
			{
				OrgBranchId = branch.OrgBranchId,
				BranchName = branch.Name
			});
		}
		return View(user);
	}

	

	[HttpPost]
	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_ADD)]
	public async Task<IActionResult> Create(UserCreateViewModel user)
	{
		if (UserSession.OrganizationId == -1)
		{
			ViewData[ViewStaticData.UserType] = EnumUserTypeOptions.Master;
		}
		else
		{
			ViewData[ViewStaticData.UserType] = EnumUserTypeOptions.Company;
			user.UserTypeId = (int)EnumUserTypeOptions.Company;
		}

		user.UserName = user.EmailAddress;
		user.IsDefaultPassword = true;
		user.CreatedTime = DateTime.Now;
		user.CreatedBy = UserSession.UserId;
		user.IsActive = true;
		user.OrganizationId = UserSession.OrganizationId;
		ModelState.Clear();
		TryValidateModel(user);
		var isExistUser = await _service.IsUserExists(user.EmailAddress);
		if (isExistUser)
		{
			ModelState.AddModelError("EmailAddress", "User already exists!!!");
		}

		var roleDt = await _roleService.Query().SelectAsync();
		var roles = roleDt.Where(c => c.OrganizationId == UserSession.OrganizationId).Select(s => new CustomSelectListItem
		{
			Id = s.RoleId,
			Name = s.RoleName
		}).ToList();
		var userTypeDt = await _userTypeService.Query().SelectAsync();
		var userTypes = userTypeDt.Select(s => new CustomSelectListItem
		{
			Id = s.UserTypeId,
			Name = s.UserTypeName
		}).ToList();
		if (ModelState.IsValid)
		{
			try
			{
				var users = await _service.Query().Where(w => w.EmailAddress.ToLower()
																	 == user.EmailAddress.ToLower())
					.SelectAsync(CancellationToken.None);
				if (users.Any())
				{
					user.Roles = roles;
					user.UserTypes = userTypes;
					TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
					return View(user);
				}

				var fileSaveDto = new FileSaveDto
				{
					FileRootPath = ControllerStaticData.FileRootPath,
					FileModulePath = ControllerStaticData.FileUploadedUserImagePath,
					OrganizationId = UserSession.OrganizationId
				};

				var fsf = await _fileOperationService.SaveFile(user.UserImage, fileSaveDto);

                var signatureFileSaveDto = new FileSaveDto
                {
                    FileRootPath = ControllerStaticData.FileRootPath,
                    FileModulePath = ControllerStaticData.FileUploadedUserSignaturePath,
                    OrganizationId = UserSession.OrganizationId
                };

                string signatureFileUrl = null;
                if (user.UserSignature != null)
                {
	                signatureFileUrl = (await _fileOperationService.SaveFile(user.UserSignature, fileSaveDto)).FileUrl;
				}
                

                if (user.IsRequireBranch == true)
                {
                    await _service.InsertUserBranches(user.UserBranchList, UserSession.OrganizationId, UserSession.UserId);
                }

                await UnitOfWork.SaveChangesAsync();

                var defaultPassword =
					_configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_DEFAULT_PASSWORD);
				var encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);
				user.Password = new PasswordGenerate().Encrypt(defaultPassword, encryptionKey);
				user.UserImageUrl = fsf.FileUrl;
				user.UserSignUrl = signatureFileUrl;
                var userData = _mapper.Map<UserCreateViewModel, User>(user);
                _service.Insert(userData);
				await UnitOfWork.SaveChangesAsync();
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;
				//user.Password = "*** (Hidden due to confidential issue)";
				var userObj = JsonConvert.SerializeObject(user, Formatting.None,
					new JsonSerializerSettings()
					{
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					});
				var au = new AuditLog
				{
					ObjectTypeId = (int)EnumObjectType.User,
					PrimaryKey = user.UserId,
					AuditOperationId = (int)EnumOperations.Add,
					UserId = UserSession.UserId,
					Datetime = DateTime.Now,
					Descriptions = userObj,
					IsActive = true,
					CreatedBy = UserSession.UserId,
					CreatedTime = DateTime.Now,
					OrganizationId = UserSession.OrganizationId
				};
				await AuditLogCreate(au);
				return RedirectToAction(ControllerStaticData.DISPLAY_INDEX);
			}
			catch (Exception ex)
			{
				TempData[ControllerStaticData.MESSAGE] = ex.Message;
			}
		}

		user.Roles = roles;
		user.UserTypes = userTypes;

		return View(user);
	}

	public async Task<IActionResult> ChangePassword()
	{
		string encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);
		var userId = UserSession.UserId;
		var userData = await _service.Query().SingleOrDefaultAsync(p => p.UserId == userId, CancellationToken.None);
		var oldPass = new PasswordGenerate().Decrypt(userData.Password, encryptionKey);
		vmUsers vm = new vmUsers
		{
			UserName = userData.UserName,
			OldPassword = oldPass
		};
		return View(vm);
	}

	[HttpPost]
	public async Task<IActionResult> ChangePassword(vmUsers user)
	{
		if (ModelState.IsValid)
		{
			string encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);

			if (user.NewPassword == user.ConfirmPassword)
			{
				var userId = UserSession.UserId;
				var userData = await _service.Query()
					.SingleOrDefaultAsync(p => p.UserId == userId, CancellationToken.None);
				var oldPass = new PasswordGenerate().Decrypt(userData.Password, encryptionKey);
				if (oldPass == user.OldPassword)
				{
					var encryPass = new PasswordGenerate().Encrypt(user.NewPassword, encryptionKey);
					userData.Password = encryPass;
					if (userData.IsDefaultPassword == true)
					{
						userData.IsDefaultPassword = false;
					}

					_service.Update(userData);
					await UnitOfWork.SaveChangesAsync();

					AuditLog au = new AuditLog
					{
						ObjectTypeId = (int)EnumObjectType.User,
						PrimaryKey = userData.UserId,
						AuditOperationId = (int)EnumOperations.Edit,
						UserId = UserSession.UserId,
						Datetime = DateTime.Now,
						Descriptions = "Password change",
						IsActive = true,
						CreatedBy = UserSession.UserId,
						CreatedTime = DateTime.Now,
						OrganizationId = UserSession.OrganizationId
					};
					await AuditLogCreate(au);
					TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.SUCCESS_CLASSNAME;

					return RedirectToAction(ControllerStaticData.DISPLAY_DASHBOARD, ControllerStaticData.HOME);
				}
				else
				{
					ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.MESSAGE_PASSWORD;
				}
			}
			else
			{
				ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.MESSAGE_PASSWORD;
			}
		}

		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
		return View(ViewStaticData.CHANGE_PASSWORD);
	}

	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_EDIT)]
	public async Task<IActionResult> Edit(string id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var user = await _service.GetUser(id);
		if (user == null)
		{
			return NotFound();
		}

		//if (UserSession.OrganizationId == -1)
		//{
		//	ViewData[ViewStaticData.UserType] = EnumUserTypeOptions.Master;
		//}
		//else
		//{
		//	ViewData[ViewStaticData.UserType] = EnumUserTypeOptions.Company;
		//}

		//var roleDt = await _roleService.Query().SelectAsync();
		//IEnumerable<CustomSelectListItem> Roles = roleDt.Where(c => c.OrganizationId == UserSession.OrganizationId).Select(
		//	s => new CustomSelectListItem
		//	{
		//		Id = s.RoleId,
		//		Name = s.RoleName
		//	});
		//var userTypeDt = await _userTypeService.Query().SelectAsync();
		//IEnumerable<CustomSelectListItem> UserTypes = userTypeDt.Select(s => new CustomSelectListItem
		//{
		//	Id = s.UserTypeId,
		//	Name = s.UserTypeName
		//});
		//user.Roles = Roles;
		//user.UserTypes = UserTypes;
		user.EncryptedId = id;
		UserSession.PreviousData = JsonConvert.SerializeObject(user, Formatting.None,
			new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
		HttpContext.Session.SetComplexData(ControllerStaticData.SESSION, UserSession);
		var model = _mapper.Map<User, UserUpdateViewModel>(user);
		return View(model);
	}

	[HttpPost]
	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_EDIT)]
	public async Task<IActionResult> Edit(UserUpdateViewModel user)
	{
		int UserId = int.Parse(IvatDataProtector.Unprotect(user.EncryptedId));
		if (UserId != user.UserId)
		{
			return BadRequest();
		}

		//if (UserSession.OrganizationId == -1)
		//{
		//	ViewData[ViewStaticData.UserType] = EnumUserTypeOptions.Master;
		//}
		//else
		//{
		//	ViewData[ViewStaticData.UserType] = EnumUserTypeOptions.Company;
		//	user.UserTypeId = (int)EnumUserTypeOptions.Company;
		//}

		var userData = await _service.GetUser(user.EncryptedId);
		if (userData == null)
		{
			return NotFound();
		}

		if (userData.EmailAddress != user.EmailAddress)
		{
			ModelState.AddModelError("EmailAddress", "Username should not be changed!");
		}

		ModelState.Clear();
		TryValidateModel(user);

		if (ModelState.IsValid)
		{
			try
			{
				var prevUser = JsonConvert.SerializeObject(userData, Formatting.None,
					new JsonSerializerSettings()
					{
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					});
				

                var usr = _mapper.Map(user, userData);
				//if (UserSession.OrganizationId == -1)
				//{
				//	userData.UserTypeId = user.UserTypeId;
				//}


				_service.Update(userData);
				await UnitOfWork.SaveChangesAsync();

				var userObj = JsonConvert.SerializeObject(userData, Formatting.None,
					new JsonSerializerSettings()
					{
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					});
				AuditLog au = new AuditLog
				{
					ObjectTypeId = (int)EnumObjectType.User,
					PrimaryKey = user.UserId,
					AuditOperationId = (int)EnumOperations.Edit,
					UserId = UserSession.UserId,
					Datetime = DateTime.Now,
					Descriptions = GetChangeInformation(prevUser.ToString(), userObj.ToString()),
					IsActive = true,
					CreatedBy = UserSession.UserId,
					CreatedTime = DateTime.Now,
					OrganizationId = UserSession.OrganizationId
				};
				await AuditLogCreate(au);
				TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
				return RedirectToAction(nameof(Index));
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await UserExists(user.UserId))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
		}

		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
		//var roleDt = await _roleService.Query().SelectAsync();
		//IEnumerable<CustomSelectListItem> Roles = roleDt.Where(c => c.OrganizationId == UserSession.OrganizationId).Select(
		//	s => new CustomSelectListItem
		//	{
		//		Id = s.RoleId,
		//		Name = s.RoleName
		//	});
		//var userTypeDt = await _userTypeService.Query().SelectAsync();
		//IEnumerable<CustomSelectListItem> UserTypes = userTypeDt.Select(s => new CustomSelectListItem
		//{
		//	Id = s.UserTypeId,
		//	Name = s.UserTypeName
		//});
		//user.Roles = Roles;
		//user.UserTypes = UserTypes;
		return View(user);
	}

	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_DELETE)]
	public async Task<IActionResult> ChangeUserStatus(string id)
	{
		var userDt = await _service.Query()
			.SingleOrDefaultAsync(p => p.UserId == int.Parse(IvatDataProtector.Unprotect(id)), CancellationToken.None);

		if (userDt.IsActive == true)
		{
			userDt.IsActive = false;
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.DELETE_CLASSNAME;
		}
		else
		{
			userDt.IsActive = true;
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;
		}

		_service.Update(userDt);
		await UnitOfWork.SaveChangesAsync();
		AuditLog au = new AuditLog
		{
			ObjectTypeId = (int)EnumObjectType.User,
			PrimaryKey = userDt.UserId,
			AuditOperationId = (int)EnumOperations.Delete,
			UserId = UserSession.UserId,
			Datetime = DateTime.Now,
			Descriptions = "IsActive:0",
			IsActive = true,
			CreatedBy = UserSession.UserId,
			CreatedTime = DateTime.Now,
			OrganizationId = UserSession.OrganizationId
		};
		await AuditLogCreate(au);

		return RedirectToAction(nameof(Index));
	}

	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_DELETE)]
	public async Task<IActionResult> Inactivate(int? id)
	{
		var userDt = await _service.Query().SingleOrDefaultAsync(p => p.UserId == id, CancellationToken.None);

		if (userDt.IsActive)
		{
			userDt.IsActive = false;
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.IN_ACTIVE_CLASSNAME;

			_service.Update(userDt);
			await UnitOfWork.SaveChangesAsync();
			AuditLog au = new AuditLog
			{
				ObjectTypeId = (int)EnumObjectType.User,
				PrimaryKey = userDt.UserId,
				AuditOperationId = (int)EnumOperations.Edit,
				UserId = UserSession.UserId,
				Datetime = DateTime.Now,
				Descriptions = "IsActive:0",
				IsActive = true,
				CreatedBy = UserSession.UserId,
				CreatedTime = DateTime.Now,
				OrganizationId = UserSession.OrganizationId
			};
			await AuditLogCreate(au);
		}

		return RedirectToAction(nameof(Index));
	}

	[VmsAuthorize(FeatureList.ADMINSTRATION)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
	[VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_DELETE)]
	public async Task<IActionResult> Activate(int? id)
	{
		var userDt = await _service.Query().SingleOrDefaultAsync(p => p.UserId == id, CancellationToken.None);

		if (!userDt.IsActive)
		{
			userDt.IsActive = true;
			TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ACTIVE_CLASSNAME;

			_service.Update(userDt);
			await UnitOfWork.SaveChangesAsync();
			AuditLog au = new AuditLog
			{
				ObjectTypeId = (int)EnumObjectType.User,
				PrimaryKey = userDt.UserId,
				AuditOperationId = (int)EnumOperations.Edit,
				UserId = UserSession.UserId,
				Datetime = DateTime.Now,
				Descriptions = "IsActive:1",
				IsActive = true,
				CreatedBy = UserSession.UserId,
				CreatedTime = DateTime.Now,
				OrganizationId = UserSession.OrganizationId
			};
			await AuditLogCreate(au);
		}

		return RedirectToAction(nameof(Index));
	}

    #region Update role

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_UPDATE_ROLE)]
    public async Task<IActionResult> UserUpdateRole(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _service.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        user.EncryptedId = id;
        UserSession.PreviousData = JsonConvert.SerializeObject(user, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        HttpContext.Session.SetComplexData(ControllerStaticData.SESSION, UserSession);
        var model = _mapper.Map<User, UserRoleUpdateViewModel>(user);
        model.Roles = await _roleService.GetRoleSelectList(UserSession.OrganizationId);
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_UPDATE_ROLE)]
    public async Task<IActionResult> UserUpdateRole(UserRoleUpdateViewModel user)
    {
        int UserId = int.Parse(IvatDataProtector.Unprotect(user.EncryptedId));
        if (UserId != user.UserId)
        {
            return BadRequest();
        }
        var userData = await _service.GetUser(user.EncryptedId);
        if (userData == null)
        {
            return NotFound();
        }

        ModelState.Clear();
        TryValidateModel(user);

        if (ModelState.IsValid)
        {
            try
            {
                var prevUser = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                var usr = _mapper.Map(user, userData);
                _service.Update(userData);
                await UnitOfWork.SaveChangesAsync();

                var userObj = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.User,
                    PrimaryKey = user.UserId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = GetChangeInformation(prevUser.ToString(), userObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await AuditLogCreate(au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
        user.Roles = await _roleService.GetRoleSelectList(UserSession.OrganizationId);

        return View(user);
    }

    #endregion

    #region Update Password

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_UPDATE_PASSWORD)]
    public async Task<IActionResult> UserUpdatePassword(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _service.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        user.EncryptedId = id;
        UserSession.PreviousData = JsonConvert.SerializeObject(user, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        HttpContext.Session.SetComplexData(ControllerStaticData.SESSION, UserSession);
        var model = _mapper.Map<User, UserPasswordUpdateViewModel>(user);
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_UPDATE_PASSWORD)]
    public async Task<IActionResult> UserUpdatePassword(UserPasswordUpdateViewModel user)
    {
        int UserId = int.Parse(IvatDataProtector.Unprotect(user.EncryptedId));
        if (UserId != user.UserId)
        {
            return BadRequest();
        }
        var userData = await _service.GetUser(user.EncryptedId);
        if (userData == null)
        {
            return NotFound();
        }

        ModelState.Clear();
        TryValidateModel(user);

        if (ModelState.IsValid)
        {
            try
            {
                var prevUser = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                var defaultPassword =
                    _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_DEFAULT_PASSWORD);
                var encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);
                user.Password = new PasswordGenerate().Encrypt(defaultPassword, encryptionKey);

                var usr = _mapper.Map(user, userData);
                _service.Update(userData);
                await UnitOfWork.SaveChangesAsync();

                var userObj = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.User,
                    PrimaryKey = user.UserId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = GetChangeInformation(prevUser.ToString(), userObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await AuditLogCreate(au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;

        return View(user);
    }

    #endregion

    #region Update Image & Signature

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_UPDATE_IMAGE)]
    public async Task<IActionResult> UserUpdateImage(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _service.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        user.EncryptedId = id;
        UserSession.PreviousData = JsonConvert.SerializeObject(user, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        HttpContext.Session.SetComplexData(ControllerStaticData.SESSION, UserSession);
        var model = _mapper.Map<User, UserImageUpdateViewModel>(user);
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_UPDATE_IMAGE)]
    public async Task<IActionResult> UserUpdateImage(UserImageUpdateViewModel user)
    {
        int UserId = int.Parse(IvatDataProtector.Unprotect(user.EncryptedId));
        if (UserId != user.UserId)
        {
            return BadRequest();
        }
        var userData = await _service.GetUser(user.EncryptedId);
        if (userData == null)
        {
            return NotFound();
        }

        ModelState.Clear();
        TryValidateModel(user);

        if (ModelState.IsValid)
        {
            try
            {
                var prevUser = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                var fileSaveDto = new FileSaveDto
                {
                    FileRootPath = ControllerStaticData.FileRootPath,
                    FileModulePath = ControllerStaticData.FileUploadedUserImagePath,
                    OrganizationId = UserSession.OrganizationId
                };

                var imageFileUrl = await _fileOperationService.SaveFile(user.UserImage, fileSaveDto);

                user.UserImageUrl = imageFileUrl.FileUrl;

                var usr = _mapper.Map(user, userData);
                _service.Update(userData);
                await UnitOfWork.SaveChangesAsync();

                var userObj = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.User,
                    PrimaryKey = user.UserId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = GetChangeInformation(prevUser.ToString(), userObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await AuditLogCreate(au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;

        return View(user);
    }


    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_UPDATE_SIGNATURE)]
    public async Task<IActionResult> UserUpdateSignature(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _service.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        user.EncryptedId = id;
        UserSession.PreviousData = JsonConvert.SerializeObject(user, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        HttpContext.Session.SetComplexData(ControllerStaticData.SESSION, UserSession);
        var model = _mapper.Map<User, UserSignatureUpdateViewModel>(user);
        return View(model);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_UPDATE_SIGNATURE)]
    public async Task<IActionResult> UserUpdateSignature(UserSignatureUpdateViewModel user)
    {
        int UserId = int.Parse(IvatDataProtector.Unprotect(user.EncryptedId));
        if (UserId != user.UserId)
        {
            return BadRequest();
        }
        var userData = await _service.GetUser(user.EncryptedId);
        if (userData == null)
        {
            return NotFound();
        }

        ModelState.Clear();
        TryValidateModel(user);

        if (ModelState.IsValid)
        {
            try
            {
                var prevUser = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });



				var signatureFileSaveDto = new FileSaveDto
				{
					FileRootPath = ControllerStaticData.FileRootPath,
					FileModulePath = ControllerStaticData.FileUploadedUserSignaturePath,
					OrganizationId = UserSession.OrganizationId
				};

				var signatureFileUrl = await _fileOperationService.SaveFile(user.UserSignature, signatureFileSaveDto);


				user.UserSignUrl = signatureFileUrl.FileUrl;

				var usr = _mapper.Map(user, userData);
                _service.Update(userData);
                await UnitOfWork.SaveChangesAsync();

                var userObj = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.User,
                    PrimaryKey = user.UserId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = GetChangeInformation(prevUser.ToString(), userObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await AuditLogCreate(au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;

        return View(user);
    }

    #endregion

    #region User Update Branch

    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_MANAGE_BRANCH)]
    public async Task<IActionResult> UserManageBranch(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _service.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }
        user.EncryptedId = id;
        UserSession.PreviousData = JsonConvert.SerializeObject(user, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        HttpContext.Session.SetComplexData(ControllerStaticData.SESSION, UserSession);
		var userBranchUpdateModel = new UserBranchUpdateViewModel();

        var branches = await _branchService.GetOrgBranchByOrganization(UserSession.ProtectedOrganizationId);

        foreach (var branch in branches)
        {
            userBranchUpdateModel.UserBranchList.Add(new UserBranchCreateViewModel
            {
                OrgBranchId = branch.OrgBranchId,
                BranchName = branch.Name
            });
        }
		userBranchUpdateModel.UserId = user.UserId;
		userBranchUpdateModel.EncryptedId = user.EncryptedId;
        //var model = _mapper.Map<User, UserBranchUpdateViewModel>(user);
        return View(userBranchUpdateModel);
    }

    [HttpPost]
    [VmsAuthorize(FeatureList.ADMINSTRATION)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_VIEW)]
    [VmsAuthorize(FeatureList.ADMINSTRATION_USERS_CAN_MANAGE_BRANCH)]
    public async Task<IActionResult> UserManageBranch(UserBranchUpdateViewModel user)
    {
        int UserId = int.Parse(IvatDataProtector.Unprotect(user.EncryptedId));
        if (UserId != user.UserId)
        {
            return BadRequest();
        }
        var userData = await _service.GetUser(user.EncryptedId);
        if (userData == null)
        {
            return NotFound();
        }

        ModelState.Clear();
        TryValidateModel(user);

        if (ModelState.IsValid)
        {
            try
            {
                var prevUser = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                if (user.IsRequireBranch == true)
                {
                    await _service.InsertUserBranches(user.UserBranchList, UserSession.OrganizationId, UserSession.UserId);
                }

                await UnitOfWork.SaveChangesAsync();

                var usr = _mapper.Map(user, userData);
                _service.Update(userData);
                await UnitOfWork.SaveChangesAsync();

                var userObj = JsonConvert.SerializeObject(userData, Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                AuditLog au = new AuditLog
                {
                    ObjectTypeId = (int)EnumObjectType.User,
                    PrimaryKey = user.UserId,
                    AuditOperationId = (int)EnumOperations.Edit,
                    UserId = UserSession.UserId,
                    Datetime = DateTime.Now,
                    Descriptions = GetChangeInformation(prevUser.ToString(), userObj.ToString()),
                    IsActive = true,
                    CreatedBy = UserSession.UserId,
                    CreatedTime = DateTime.Now,
                    OrganizationId = UserSession.OrganizationId
                };
                await AuditLogCreate(au);
                TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.UPDATE_CLASSNAME;
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;

        return View(user);
    }

    #endregion



    private async Task<bool> UserExists(int id)
	{
		return await _service.Query().AnyAsync(e => e.UserId == id, CancellationToken.None);
	}
}