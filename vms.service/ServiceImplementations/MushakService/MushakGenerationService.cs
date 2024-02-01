using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.entity.viewModels.ReportsViewModel;
using vms.repository.Repository.tbl;
using vms.service.Services.MushakService;

namespace vms.service.ServiceImplementations.MushakService;

public class MushakGenerationService : ServiceBase<MushakGeneration>, IMushakGenerationService
{
    private readonly IMushakGenerationRepository _repository;

    public MushakGenerationService(IMushakGenerationRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MushakGeneration>> GetMushakGenerations(int orgIdEnc)
    {
        return await _repository.GetMushakGenerations(orgIdEnc);
    }

    public async Task<MushakGeneration> GetMushakGeneration(string idEnc)
    {
        return await _repository.GetMushakGeneration(idEnc);
    }


    public async Task<bool> InsertMushak(SpAddMushakReturnBasicInfo vm)
    {
        return await _repository.InsertMushak(vm);
    }

    public async Task<bool> InsertReturnPlanToPaymentInfo(SpAddMushakReturnPlanToPaymentInfo model)
    {
        return await _repository.InsertReturnPlanToPaymentInfo(model);
    }

    public async Task<bool> InsertReturnPaymentInfo(SpAddMushakReturnPaymentInfo model)
    {
        return await _repository.InsertReturnPaymentInfo(model);
    }

    public async Task<bool> InsertSubmissionInfo(SpAddMushakReturnSubmissionInfo vm)
    {
        return await _repository.InsertSubmissionInfo(vm);
    }

    public async Task<bool> InsertReturnReturnedAmountInfo(SpAddMushakReturnReturnedAmountInfo model)
    {
        return await _repository.InsertReturnReturnedAmountInfo(model);
    }

    public async Task<bool> InsertAddMushakReturnCompleteProcess(int id)
    {
        return await _repository.InsertAddMushakReturnCompleteProcess(id);
    }

    public async Task<List<SpPurchaseCalcBook>> Mushak6P1(int productId, int orgId, string fromDate, string toDate,
        int orgBranchId = 0)
    {
        return await _repository.Mushak6P1(productId, orgId, orgBranchId, fromDate, toDate);
    }

    public async Task<List<SpPurchaseSaleCalcBook>> Mushak6P2P1(int productId, int orgId, int branchId, string fromDate,
        string toDate, int vendorId = 0, int customerId = 0)
    {
        return await _repository.Mushak6P2P1(productId, orgId, branchId, vendorId, customerId, fromDate, toDate);
    }

    public async Task<List<SpSalesCalcBook>> Mushak6P2(int productId, int orgId, string fromDate, string toDate,
        int orgBranchId = 0)
    {
        return await _repository.Mushak6P2(productId, orgId, orgBranchId, fromDate, toDate);
    }

    public async Task<List<Sp4p3>> Mushak4P3(int PriceDeclarId = 0)
    {
        return await _repository.Mushak4P3(PriceDeclarId);
    }

    public async Task<VmMushakHighValue> Mushak6P10(int organizationId, int year, int month,
        decimal? lowerLimitOfHighValSale = 200000)
    {
        return await _repository.Mushak6P10(organizationId, year, month, lowerLimitOfHighValSale);
    }

    public async Task<List<SpDebitMushak>> Mushak6P8(int DebitNoteId)
    {
        return await _repository.Mushak6P8(DebitNoteId);
    }

    public async Task<List<SpSalesTaxInvoice>> Mushak6P3(int SalesId)
    {
        return await _repository.Mushak6P3(SalesId);
    }

    public async Task<List<SpCreditMushak>> Mushak6P7(int CreditNoteId)
    {
        return await _repository.Mushak6P7(CreditNoteId);
    }

    public async Task<List<SpVdsPurchaseCertificate>> Mushak6P6(int PurchaseId)
    {
        return await _repository.Mushak6P6(PurchaseId);
    }

    public async Task<List<SpBranchTransfer>> Mushak6P5(int SaleId)
    {
        return await _repository.Mushak6P5(SaleId);
    }

    public async Task<List<SpGetTdsPurchaseCertificate>> GetTdsCertificate(int purchaseId)
    {
        return await _repository.GetTdsCertificate(purchaseId);
    }
}