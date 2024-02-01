using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IOverHeadCostRepository : IRepositoryBase<OverHeadCost>
{
    Task<IEnumerable<OverHeadCost>> GetOverHeadCost(int orgIdEnc);
}