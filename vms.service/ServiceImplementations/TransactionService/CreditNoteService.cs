using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class CreditNoteService : ServiceBase<CreditNote>, ICreditNoteService
{
    private ICreditNoteRepository _repository;
    public CreditNoteService(ICreditNoteRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<ViewCreditNote>> GetCreditNoteData(string orgIdEnc)
    {
        return _repository.GetCreditNoteData(orgIdEnc);
    }
    public Task<IEnumerable<ViewCreditNote>> GetCreditNoteDataByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        return _repository.GetCreditNoteDataByOrganizationAndBranch(orgIdEnc, branchIds, isRequiredBranch);
    }
}