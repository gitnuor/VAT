using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel.ParamModel;
using vms.entity.viewModels;

namespace vms.repository.Repository.tbl;

public interface IPurchaseOrderRepository : IRepositoryBase<Purchase>
{
    Task<bool> InsertData(VmPurchase purchase);

    Task<bool> InsertDebitNote(vmDebitNote vmDebitNote);

    Task<bool> ManagePurchaseDue(vmPurchasePayment vmPurchase);

    Task<Purchase> GetPurchaseDetails(string idEnc);

    Task<IEnumerable<Purchase>> GetPurchaseDue(int orgId);
    Task<IEnumerable<Purchase>> GetPurchaseDueByOrgAndBranch(int orgId, List<int> branchIds, bool isRequirdBranch);

    Task<bool> InsertTransferReceive(vmTransferReceive vm);

    Task<IEnumerable<Purchase>> GetPurchases(int orgIdEnc);
    Task<int> InsertLocalPurchase(SpInsertPurchaseCombinedParam purchase);
    Task<int> InsertPurchase(SpInsertPurchaseCombinedParam purchase);
}