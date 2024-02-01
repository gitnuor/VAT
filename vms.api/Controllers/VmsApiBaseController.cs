using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using URF.Core.Abstractions;
using vms.api.VmsApiUtility;
using vms.entity.viewModels;
using vms.service.Services.SecurityService;
using vms.service.Services.SettingService;

namespace vms.api.Controllers;

public class VmsApiBaseController : ControllerBase
{
	protected IWebHostEnvironment WhEnvironment;
	protected IUnitOfWork BaseUnitOfWork;
	protected IDataProtector VatDataProtector;
	protected IAuditLogService VatLogService;
	protected CurrentUserViewModel CurrentUser;
	protected IIntegratedApplicationService IntegratedAppSvc;


	public VmsApiBaseController(ControllerBaseParamModel baseModel)
	{
		WhEnvironment = baseModel.HostingEnvironment;
		BaseUnitOfWork = baseModel.UnitOfWork;
		VatDataProtector =
			baseModel.ProtectionProvider.CreateProtector(baseModel.PurposeString
				.UserIdQueryString);
		VatLogService = baseModel.AuditLogService;
		IntegratedAppSvc = baseModel.IntegratedApplicationService;
		CurrentUser = new CurrentUserViewModel
		{
			UserId = 10,
			OrganizationId = 7,
			ProtectedOrganizationId = VatDataProtector.Protect("7")
		};

	}
}