using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface IAdjustmentService : IServiceBase<Adjustment>
{
    Task<IEnumerable<Adjustment>> Get(int organizationId, string searchQuery = null);
    Task<IEnumerable<Adjustment>> GetByOrg(string pOrgId);
}