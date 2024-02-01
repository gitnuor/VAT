using Microsoft.AspNetCore.DataProtection;
using URF.Core.Abstractions;
using vms.entity.viewModels;
using vms.service.Services.SecurityService;
using vms.service.Services.SettingService;

namespace vms.api.VmsApiUtility;

public class ControllerBaseParamModel
{
	public ControllerBaseParamModel(IUnitOfWork unitOfWork, IDataProtectionProvider protectionProvider,
		PurposeStringConstants purposeString, IWebHostEnvironment hostingEnvironment, IAuditLogService auditLogService,
		IIntegratedApplicationService integratedApplicationService)
	{
		UnitOfWork = unitOfWork;
		ProtectionProvider = protectionProvider;
		PurposeString = purposeString;
		AuditLogService = auditLogService;
		IntegratedApplicationService = integratedApplicationService;
		HostingEnvironment = hostingEnvironment;
	}

	public IWebHostEnvironment HostingEnvironment { get; }
	public IUnitOfWork UnitOfWork { get; }
	public IDataProtectionProvider ProtectionProvider { get; }
	public PurposeStringConstants PurposeString { get; }
	public IAuditLogService AuditLogService { get; }
	public IIntegratedApplicationService IntegratedApplicationService { get; }
}