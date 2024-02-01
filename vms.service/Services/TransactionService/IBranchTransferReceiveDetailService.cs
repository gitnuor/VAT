using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface IBranchTransferReceiveDetailService : IServiceBase<BranchTransferReceiveDetail>
{
    Task<IEnumerable<BranchTransferReceiveDetail>> GetBranchTransferReceiveDetailsByBranchTransferReceive(string branchTransferReceiveIdEnc);
}