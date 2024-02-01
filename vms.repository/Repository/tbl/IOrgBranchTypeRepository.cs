using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IOrgBranchTypeRepository : IRepositoryBase<OrgBranchType>
{
    Task<IEnumerable<OrgBranchType>> GetOrgBranchType();
}