using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class DebitNoteService : ServiceBase<DebitNote>, IDebitNoteService
{
    private readonly IDebitNoteRepository _repository;
    public DebitNoteService(IDebitNoteRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<ViewDebitNote>> GetDebitNoteData(string orgIdEnc)
    {
        return _repository.GetDebitNoteData(orgIdEnc);
    }

    public Task<IEnumerable<ViewDebitNote>> GetDebitNoteDataByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return _repository.GetDebitNoteDataByOrgAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }
}