using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SecurityService;

namespace vms.service.ServiceImplementations.SecurityService;

public class AuditOperationService : ServiceBase<AuditOperation>, IAuditOperationService
{
    public AuditOperationService(IAuditOperationRepository repository) : base(repository)
    {
    }
}