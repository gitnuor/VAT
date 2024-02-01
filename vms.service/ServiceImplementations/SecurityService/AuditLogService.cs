using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SecurityService;

namespace vms.service.ServiceImplementations.SecurityService;

public class AuditLogService : ServiceBase<AuditLog>, IAuditLogService
{
    private readonly IAuditLogRepository _repository;
    public AuditLogService(IAuditLogRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public Task<bool> RestoreAudit(vmRestore vmRestore)
    {
        return _repository.RestoreAuditAsync(vmRestore);
    }

    public async Task<IEnumerable<AuditLog>> GetAuditLogs(int orgIdEnc)
    {
        return await _repository.GetAuditLogs(orgIdEnc);
    }
    public async Task<AuditLog> GetAuditLog(string idEnc)
    {
        return await _repository.GetAuditLog(idEnc);
    }


}