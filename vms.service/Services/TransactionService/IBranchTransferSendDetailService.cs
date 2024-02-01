using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface IBranchTransferSendDetailService : IServiceBase<BranchTransferSendDetail>
{
    Task<IEnumerable<BranchTransferSendDetail>> GetBranchTransferSendDetailsByBranchTransferSend(string branchTransferSendIdEnc);
}