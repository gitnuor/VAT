using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IAdjustmentRepository : IRepositoryBase<Adjustment>
{
    Task<IEnumerable<Adjustment>> GetByOrg(string pOrgId);
}