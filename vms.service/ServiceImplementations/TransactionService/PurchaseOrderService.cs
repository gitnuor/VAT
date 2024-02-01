using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.StoredProcedureModel.ParamModel;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class PurchaseOrderService : ServiceBase<Purchase>, IPurchaseOrderService
{
    private readonly IMapper _mapper;
    private readonly IPurchaseOrderRepository _repository;

    public PurchaseOrderService(IPurchaseOrderRepository repository, IMapper mapper) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<bool> InsertData(VmPurchase purchase)
    {
        return await _repository.InsertData(purchase);
    }

    public async Task<int> InsertLocalPurchase(VmPurchaseLocalPost purchase)
    {
        var purc = _mapper.Map<VmPurchaseLocalPost, SpInsertPurchaseCombinedParam>(purchase);
        return await _repository.InsertPurchase(purc);
    }


    public int InsertImportPurchase(VmPurchaseImportPost purchase)
    {
        var purc = _mapper.Map<VmPurchaseImportPost, SpInsertPurchaseCombinedParam>(purchase);
        return 1;
    }
    public async Task<bool> InsertDebitNote(vmDebitNote vmDebitNote)
    {
        return await _repository.InsertDebitNote(vmDebitNote);
    }


    public async Task<bool> InsertDebitNote(VmPurchaseDebitNotePost vmDebitNote)
    {
        var purchaseDebitNote = _mapper.Map<VmPurchaseDebitNotePost, vmDebitNote>(vmDebitNote);
        return await _repository.InsertDebitNote(purchaseDebitNote);
    }

    public async Task<bool> ManagePurchaseDue(vmPurchasePayment vmPurchase)
    {
        return await _repository.ManagePurchaseDue(vmPurchase);
    }

    public async Task<Purchase> GetPurchaseDetails(string idEnc)
    {
        return await _repository.GetPurchaseDetails(idEnc);

    }

    public async Task<IEnumerable<Purchase>> GetPurchaseDue(int orgId)
    {
        return await _repository.GetPurchaseDue(orgId);
    }

    public async Task<IEnumerable<Purchase>> GetPurchaseDueByOrgAndBranch(int orgId, List<int> branchIds, bool isRequirdBranch)
    {
        return await _repository.GetPurchaseDueByOrgAndBranch(orgId, branchIds, isRequirdBranch);
    }

    public async Task<bool> InsertTransferReceive(vmTransferReceive vm)
    {
        return await _repository.InsertTransferReceive(vm);
    }

    public async Task<IEnumerable<Purchase>> GetPurchases(int orgIdEnc)
    {
        return await _repository.GetPurchases(orgIdEnc);
    }

    public async Task<IEnumerable<Purchase>> GetPurchasesIncludingOtherTables(int orgIdEnc)
    {
        return await _repository.Query().Where(c => c.OrganizationId == orgIdEnc).Include(c => c.Organization).Include(c => c.PurchaseType).Include(c => c.DebitNotes).Include(c => c.Vendor).OrderByDescending(c => c.PurchaseId)
            .SelectAsync();
    }
}