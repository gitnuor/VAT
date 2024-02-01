using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IBranchTransferReceiveDetailRepository : IRepositoryBase<BranchTransferReceiveDetail>
{
    Task<IEnumerable<BranchTransferReceiveDetail>> GetBranchTransferReceiveDetailsByBranchTransferReceive(string branchTransferReceiveIdEnc);
}