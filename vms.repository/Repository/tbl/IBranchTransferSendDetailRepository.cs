using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IBranchTransferSendDetailRepository : IRepositoryBase<BranchTransferSendDetail>
{
    Task<IEnumerable<BranchTransferSendDetail>> GetBranchTransferSendDetailsByBranchTransferSend(string branchTransferSendIdEnc);
}