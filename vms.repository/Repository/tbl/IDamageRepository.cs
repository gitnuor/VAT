using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IDamageRepository : IRepositoryBase<Damage>
{
    Task<IEnumerable<Damage>> GetDamage(int orgIdEnc);
    Task<Damage> GetDamage(string idEnc);
    Task<Damage> GetDamagewithDetails(int orgId, int damageId);
}