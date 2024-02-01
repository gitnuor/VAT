using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using URF.Core.Abstractions;
using vms.entity.models;
using vms.entity.viewModels;
using vms.utility;
using vms.utility.StaticData;
using vms.Utility;
using vms.entity.viewModels.User;
using AutoMapper;
using vms.service.Services.SecurityService;

namespace vms.Controllers;

public class AuthenticationController : Controller
{
	private readonly IUserService _service;
	private readonly IRoleRightService _roleRightService;
	private readonly IUserLoginHistoryService _userLoginHistoryService;
	private readonly IConfiguration _configuration;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDataProtector _dataProtector;
    private readonly IUserBranchService _userBranchService;
    private readonly IMapper _mapper;
    

    public AuthenticationController(IUnitOfWork unitOfWork,
		IConfiguration configuration,
		IUserService service,
		IRoleRightService roleRightService, PurposeStringConstants purposeStringConstants,
		IDataProtectionProvider dataProtectionProvider, IUserLoginHistoryService userLoginHistoryService, IUserBranchService userBranchService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_service = service;
		_roleRightService = roleRightService;
		_userLoginHistoryService = userLoginHistoryService;
		_dataProtector = dataProtectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
		_configuration = configuration;
		_userBranchService = userBranchService;
        _mapper = mapper;
    }

	public IActionResult Index()
	{
		HttpContext.Session.Clear();
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Index(VmUserLogin user)
	{
		//#region Captcha
		
		//var recaptchaHelper = this.GetRecaptchaVerificationHelper();
		
		//if (string.IsNullOrEmpty(recaptchaHelper.Response))
		//{
		//	TempData[ControllerStaticData.MESSAGE] = "Please complete the Captcha.";
		//	return View();
		//}
		
		//var recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
		//if (!recaptchaResult.Success)
		//{
		//	var errorCount = 0;
		//	var errorMessage = string.Empty;
		//	foreach (var err in recaptchaResult.ErrorCodes)
		//	{
		//		if (errorCount > 0)
		//			errorMessage += "; ";
		//		errorMessage += err;
		//		errorCount++;
		//	}
		
		//	TempData[ControllerStaticData.MESSAGE] = errorMessage;
		//	return View();
		//}
		
		//#endregion



		if (!ModelState.IsValid)
		{
			return View();
		}

		var encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);
		var userData = await _service.GetUserByUsername(user.UserName);
		if (userData == null)
		{
			TempData[ControllerStaticData.MESSAGE] = MessageStaticData.INVALID_USERNAME_OR_PASS;
			return View();
		}


      
        //var branches;
        
        var userDataViewModel = _mapper.Map<UserCreateViewModel>(userData);
        if (userData.IsRequireBranch == true)
        {
            var branches = await _userBranchService.Query().Where(x => x.UserId == userData.UserId && x.OrganizationId == userData.OrganizationId).SelectAsync();
			userDataViewModel.UserBranches = branches.ToList();
        }
      

        var clientInformation = Request.Headers.Aggregate(string.Empty,
			(current, header) => current + (header.Key + "=" + header.Value + "<br />"));

		var loginHistory = new UserLoginHistory
		{
			OrganizationId = userData.OrganizationId ?? 0,
			UserId = userData.UserId,
			IsLoginAttemptSuccess = true,
			LoginTime = DateTime.Now,
			UserClientDetailInformation = Request.Headers.Where(h => h.Key != "Cookie").Aggregate(string.Empty,
				(current, header) => current + (header.Key + "=" + header.Value + "<br />")),
			UserCookie = Request.Headers.FirstOrDefault(h => h.Key == "Cookie").Value
		};

		if (userData.Password != new PasswordGenerate().Encrypt(user.Password, encryptionKey))
        {
            TempData[ControllerStaticData.MESSAGE] = MessageStaticData.INVALID_USERNAME_OR_PASS;
				//$"{MessageStaticData.INVALID_PASSWORD} Failed attempt {userData.AccessFailedCount + 1}";
			await _service.AddFailedCountAndLockUser(userData.UserId);
			loginHistory.IsLoginAttemptSuccess = false;
			loginHistory.ReasonOfFail = MessageStaticData.INVALID_PASSWORD;
			await _userLoginHistoryService.CustomInsertAsync(loginHistory);
			await _unitOfWork.SaveChangesAsync();
			return View();
		}

		if (!userData.IsActive)
		{
			TempData[ControllerStaticData.MESSAGE] = MessageStaticData.INACTIVE_USER;
			await _service.AddFailedCount(userData.UserId);
			loginHistory.IsLoginAttemptSuccess = false;
			loginHistory.ReasonOfFail = MessageStaticData.INACTIVE_USER;
			await _userLoginHistoryService.CustomInsertAsync(loginHistory);
			await _unitOfWork.SaveChangesAsync();
			return View();
		}

		if (userData.IsLocked == true)
		{
			TempData[ControllerStaticData.MESSAGE] = MessageStaticData.LOCKED_USER;
			await _service.AddFailedCount(userData.UserId);
			loginHistory.IsLoginAttemptSuccess = false;
			loginHistory.ReasonOfFail = MessageStaticData.LOCKED_USER;
			await _userLoginHistoryService.CustomInsertAsync(loginHistory);
			await _unitOfWork.SaveChangesAsync();
			return View();
		}

		if (userData.ExpiryDate != null && userData.ExpiryDate < DateTime.Now)
		{
			TempData[ControllerStaticData.MESSAGE] = MessageStaticData.EXPIRY_DATE;
			await _service.AddFailedCount(userData.UserId);
			loginHistory.IsLoginAttemptSuccess = false;
			loginHistory.ReasonOfFail = MessageStaticData.EXPIRY_DATE;
			await _userLoginHistoryService.CustomInsertAsync(loginHistory);
			await _unitOfWork.SaveChangesAsync();
			return View();
		}

       

        var session = GetUserSession(userDataViewModel);


		if (session.UserId == 1)
		{
			session.RoleId = 11;
			//todo: for nbr approval
			session.OrganizationId = 7;
			session.ProtectedOrganizationId = _dataProtector.Protect("7");
		}

		var roleRights = await _roleRightService.GetByRole(session.RoleId);
		var rights = roleRights.Select(x => new Right
		{
			RightId = x.RightId,
			RightName = x.Right.RightName
		}).ToList();
		session.Rights = rights;

		

		HttpContext.Session.SetComplexData(ControllerStaticData.SESSION, session);
		await _service.ResetFailedCountAndUpdateLoginTime(userData.UserId);
		await _userLoginHistoryService.CustomInsertAsync(loginHistory);
		await _unitOfWork.SaveChangesAsync();
		if (userData.IsDefaultPassword)
		{
			return RedirectToAction("ChangePassword", "Users", userData);
		}

		return userData.OrganizationId == 100
			? RedirectToAction("Index", "Organizations")
			: RedirectToAction(ControllerStaticData.DISPLAY_DASHBOARD, ControllerStaticData.HOME);
	}


	public async Task<IActionResult> ChangePassword(User user)
	{
		string encryptionKey = _configuration.GetValue<string>(ControllerStaticData.PRIVATE_DATA_ENCRYPTION_KEY);
		var userId = user.UserId;
		var userData = await _service.Query().SingleOrDefaultAsync(p => p.UserId == userId, CancellationToken.None);
		var oldPass = new PasswordGenerate().Decrypt(userData.Password, encryptionKey);
		vmUsers vm = new vmUsers
		{
			UserName = userData.UserName,
			OldPassword = oldPass,
			uid = userData.UserId
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
				var userId = user.uid;
				var userData = await _service.Query()
					.SingleOrDefaultAsync(p => p.UserId == userId, CancellationToken.None);
				var encryPass = new PasswordGenerate().Encrypt(user.NewPassword, encryptionKey);
				userData.Password = encryPass;
				if (userData.IsDefaultPassword)
				{
					userData.IsDefaultPassword = false;
				}

				_service.Update(userData);

				await _unitOfWork.SaveChangesAsync();

				TempData[ControllerStaticData.MESSAGE] = "Password Updated Sussessfully .Please Login";

				return RedirectToAction("Index", ControllerStaticData.AUTHENTICATION);
			}
			else
			{
				ViewData[ControllerStaticData.MESSAGE] = MessageStaticData.MESSAGE_PASSWORD;
			}
		}

		TempData[ControllerStaticData.MESSAGE] = ControllerStaticData.ERROR_CLASSNAME;
		return View(ViewStaticData.CHANGE_PASSWORD);
	}

	private RedirectToActionResult RedirectByUser()
	{
		return RedirectToAction(ControllerStaticData.DISPLAY_DASHBOARD, ControllerStaticData.HOME);
	}

	private VmUserSession GetUserSession(UserCreateViewModel userData)
	{
		return new VmUserSession
		{
			UserId = userData.UserId,
			UserName = userData.UserName,
			RoleId = userData.RoleId,
			RoleName = userData.Role.RoleName,
			OrganizationId = userData.OrganizationId ?? 0,
			ProtectedOrganizationId = _dataProtector.Protect((userData.OrganizationId ?? 0).ToString()),
			OrganizationName = userData.Organization.Name,
			OrganizationAddress = userData.Organization.Address,
			IsProductionCompany = userData.Organization.IsProductionCompany == true,
			IsImposeServiceCharge = userData.Organization.IsImposeServiceCharge == true,
			ServiceChargePercent = userData.Organization.IsImposeServiceCharge == true
				? userData.Organization.ServiceChargePercent ?? 0
				: 0,
			IsSaleSimplified = userData.Organization.IsSaleSimplified == true,
			IsRequireGoodsId = userData.Organization.IsRequireGoodsId == true,
			IsRequireSkuId = userData.Organization.IsRequireSkuId == true,
			IsRequireSkuNo = userData.Organization.IsRequireSku == true,
			IsRequirePartNo = userData.Organization.IsRequirePartNo == true,
			InvoiceNameEng = string.IsNullOrEmpty(userData.Organization.InvoiceNameEng)
				? "Invoice No"
				: userData.Organization.InvoiceNameEng,
			InvoiceNameBan = string.IsNullOrEmpty(userData.Organization.InvoiceNameBan)
				? "ইনভয়েস নং"
				: userData.Organization.InvoiceNameBan,
			UserImageUrl = StringGenerator.RemoveWwwRoot(userData.UserImageUrl),
            IsRequireBranch = userData.IsRequireBranch,
            BranchIds = userData.UserBranches.Select(x => x.OrgBranchId).ToList(),

        };
	}
}