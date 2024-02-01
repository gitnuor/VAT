using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.TransactionService;

public interface IPurchaseOrderService : IServiceBase<Purchase>
{
    Task<bool> InsertData(VmPurchase purchase);
    Task<int> InsertLocalPurchase(VmPurchaseLocalPost purchase);
    int InsertImportPurchase(VmPurchaseImportPost purchase);
    Task<bool> ManagePurchaseDue(vmPurchasePayment vmPurchase);
    Task<bool> InsertDebitNote(vmDebitNote vmDebitNote);
    Task<bool> InsertDebitNote(VmPurchaseDebitNotePost vmDebitNote);
    Task<Purchase> GetPurchaseDetails(string idEnc);
    Task<IEnumerable<Purchase>> GetPurchaseDue(int orgId);
    Task<IEnumerable<Purchase>> GetPurchaseDueByOrgAndBranch(int orgId, List<int> branchIds, bool isRequirdBranch);
    Task<bool> InsertTransferReceive(vmTransferReceive vm);
    Task<IEnumerable<Purchase>> GetPurchases(int orgIdEnc);
    Task<IEnumerable<Purchase>> GetPurchasesIncludingOtherTables(int orgIdEnc);
}