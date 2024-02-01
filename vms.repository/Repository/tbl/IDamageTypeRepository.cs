using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IDamageTypeRepository : IRepositoryBase<DamageType>
{
    Task<IEnumerable<DamageType>> GetDamageTypes(int orgIdEnc);
    Task<DamageType> GetDamageType(string idEnc);
}