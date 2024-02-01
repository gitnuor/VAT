using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IBranchTransferReceiveRepository : IRepositoryBase<BranchTransferReceive>
{
    Task<IEnumerable<BranchTransferReceive>> GetBranchTransferReceivesByOrganization(string orgIdEnc);
}