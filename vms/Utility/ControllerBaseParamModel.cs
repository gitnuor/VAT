using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using URF.Core.Abstractions;
using vms.entity.viewModels;
using vms.service.Services.SecurityService;

namespace vms.Utility;

public class ControllerBaseParamModel
{
    public ControllerBaseParamModel(PurposeStringConstants purposeStringConstants,IDataProtectionProvider protectionProvider, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IAuditLogService auditLogService)
    {
        HostingEnvironment = hostingEnvironment;
        HttpContextAccessor = httpContextAccessor;
        UnitOfWork = unitOfWork;
        ProtectionProvider = protectionProvider;
        VatPurposeStringConstants = purposeStringConstants;
        VatAuditLogService = auditLogService;
    }

    public IWebHostEnvironment HostingEnvironment { get; }
    public IHttpContextAccessor HttpContextAccessor { get; }
    public IUnitOfWork UnitOfWork { get; }
    public IDataProtectionProvider ProtectionProvider { get; }
    public PurposeStringConstants VatPurposeStringConstants { get; }
    public IAuditLogService VatAuditLogService { get; }

}