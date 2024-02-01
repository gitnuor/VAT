using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IDamageDetailRepository : IRepositoryBase<DamageDetail>
{
    Task<IEnumerable<DamageDetail>> GetDamageDetails(int damageId);
}