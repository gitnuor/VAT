using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IOrgBranchRepository : IRepositoryBase<OrgBranch>
{
    Task<IEnumerable<OrgBranch>> GetOrgBranchByOrgId(string pOrgId);
    Task<IEnumerable<OrgBranch>> GetOrgBranchWithOutSelf(string pOrgId, int selfBranchId);
    Task<IEnumerable<OrgBranch>> GetOrgBranch();
    Task<OrgBranch> GetOrgBranch(string idEnc);
    Task<IEnumerable<OrgBranch>> GetOrgBranchByOrgIdAndUser(string pOrgId, List<int> branchIds, bool isRequiredBranch);
}