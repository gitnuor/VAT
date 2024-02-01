using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface IBranchTransferReceiveService : IServiceBase<BranchTransferReceive>
{
    Task<IEnumerable<BranchTransferReceive>> GetBranchTransferReceivesByOrganization(string orgIdEnc);
}