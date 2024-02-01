using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class BranchTransferReceiveDetailService : ServiceBase<BranchTransferReceiveDetail>, IBranchTransferReceiveDetailService
{
    private readonly IBranchTransferReceiveDetailRepository _repository;
    public BranchTransferReceiveDetailService(IBranchTransferReceiveDetailRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BranchTransferReceiveDetail>> GetBranchTransferReceiveDetailsByBranchTransferReceive(string branchTransferReceiveIdEnc)
    {
        return await _repository.GetBranchTransferReceiveDetailsByBranchTransferReceive(branchTransferReceiveIdEnc);
    }
}