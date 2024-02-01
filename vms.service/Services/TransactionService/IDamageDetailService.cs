using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface IDamageDetailService : IServiceBase<DamageDetail>
{
    Task<IEnumerable<DamageDetail>> Get(int organizationId, string searchQuery = null);
    Task<IEnumerable<DamageDetail>> GetDamageDetails(int damageId);
}