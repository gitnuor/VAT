using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.BranchTransferSend;

namespace vms.repository.Repository.tbl;

public interface IBranchTransferSendRepository : IRepositoryBase<BranchTransferSend>
{
    Task<IEnumerable<BranchTransferSend>> GetBranchTransferSendsByOrganization(string orgIdEnc);
    Task<int> InsertBranchTransferSend(BranchTransferSendParamViewModel model);
    Task<SpGetBranchTransferChallanModel> GetBranchTransferChallan(string transferIdEnc, int userId);
}