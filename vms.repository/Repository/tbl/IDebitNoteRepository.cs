using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IDebitNoteRepository : IRepositoryBase<DebitNote>
{
    Task<IEnumerable<ViewDebitNote>> GetDebitNoteData(string orgIdEnc);
    Task<IEnumerable<ViewDebitNote>> GetDebitNoteDataByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
}