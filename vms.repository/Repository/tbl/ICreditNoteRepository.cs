using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface ICreditNoteRepository : IRepositoryBase<CreditNote>
{
    Task<IEnumerable<ViewCreditNote>> GetCreditNoteData(string orgIdEnc);
    Task<IEnumerable<ViewCreditNote>> GetCreditNoteDataByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
}