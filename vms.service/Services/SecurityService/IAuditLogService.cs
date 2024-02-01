using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SecurityService;

public interface IAuditLogService : IServiceBase<AuditLog>
{
    Task<bool> RestoreAudit(vmRestore vmRestore);

    Task<IEnumerable<AuditLog>> GetAuditLogs(int orgIdEnc);
    Task<AuditLog> GetAuditLog(string idEnc);
}