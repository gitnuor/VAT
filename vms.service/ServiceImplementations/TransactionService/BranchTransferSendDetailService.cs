using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class BranchTransferSendDetailService : ServiceBase<BranchTransferSendDetail>, IBranchTransferSendDetailService
{
    private readonly IBranchTransferSendDetailRepository _repository;
    public BranchTransferSendDetailService(IBranchTransferSendDetailRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BranchTransferSendDetail>> GetBranchTransferSendDetailsByBranchTransferSend(string branchTransferSendIdEnc)
    {
        return await _repository.GetBranchTransferSendDetailsByBranchTransferSend(branchTransferSendIdEnc);
    }
}