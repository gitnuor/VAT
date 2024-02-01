using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.BranchTransferSend;

namespace vms.service.Services.TransactionService;

public interface IBranchTransferSendService : IServiceBase<BranchTransferSend>
{
    Task<IEnumerable<BranchTransferSend>> GetBranchTransferSendsByOrganization(string orgIdEnc);
    Task<int> InsertBranchTransferSend(BranchTransferSendCreatePostViewModel model);
    Task<SpGetBranchTransferChallanModel> GetBranchTransferChallan(string transferIdEnc, int userId);
}