using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface ICreditNoteService : IServiceBase<CreditNote>
{
    Task<IEnumerable<ViewCreditNote>> GetCreditNoteData(string orgIdEnc);
    Task<IEnumerable<ViewCreditNote>> GetCreditNoteDataByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
}