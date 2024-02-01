using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class BranchTransferReceiveService : ServiceBase<BranchTransferReceive>, IBranchTransferReceiveService
{
    private readonly IBranchTransferReceiveRepository _repository;
    public BranchTransferReceiveService(IBranchTransferReceiveRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BranchTransferReceive>> GetBranchTransferReceivesByOrganization(string orgIdEnc)
    {
        return await _repository.GetBranchTransferReceivesByOrganization(orgIdEnc);
    }
}