using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.repository.Repository.tbl;

public interface IAuditLogRepository : IRepositoryBase<AuditLog>
{
    Task<bool> RestoreAuditAsync(vmRestore vmRestore);

    Task<IEnumerable<AuditLog>> GetAuditLogs(int orgIdEnc);
    Task<AuditLog> GetAuditLog(string idEnc);
}