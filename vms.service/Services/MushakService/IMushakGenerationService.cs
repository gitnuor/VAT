using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.service.Services.MushakService;

public interface IMushakGenerationService : IServiceBase<MushakGeneration>
{

    Task<IEnumerable<MushakGeneration>> GetMushakGenerations(int orgIdEnc);
    Task<MushakGeneration> GetMushakGeneration(string idEnc);


    Task<bool> InsertMushak(SpAddMushakReturnBasicInfo vm);
    Task<bool> InsertReturnPlanToPaymentInfo(SpAddMushakReturnPlanToPaymentInfo model);
    Task<bool> InsertReturnPaymentInfo(SpAddMushakReturnPaymentInfo model);
    Task<bool> InsertSubmissionInfo(SpAddMushakReturnSubmissionInfo model);
    Task<bool> InsertReturnReturnedAmountInfo(SpAddMushakReturnReturnedAmountInfo model);
    Task<bool> InsertAddMushakReturnCompleteProcess(int id);
    Task<List<SpPurchaseCalcBook>> Mushak6P1(int productId, int orgId, string fromDate, string toDate, int orgBranchId);
    Task<List<SpPurchaseSaleCalcBook>> Mushak6P2P1(int productId, int orgId, int branchId, string fromDate, string toDate, int vendorId, int customerId);
    Task<List<SpSalesCalcBook>> Mushak6P2(int productId, int orgId, string fromDate, string toDate, int orgBranchId);
    Task<List<Sp4p3>> Mushak4P3(int PriceDeclarId);
    Task<VmMushakHighValue> Mushak6P10(int organizationId, int year, int month, decimal? lowerLimitOfHighValSale);
    Task<List<SpDebitMushak>> Mushak6P8(int DebitNoteId);
    Task<List<SpSalesTaxInvoice>> Mushak6P3(int SalesId);
    Task<List<SpCreditMushak>> Mushak6P7(int CreditNoteId);
    Task<List<SpVdsPurchaseCertificate>> Mushak6P6(int PurchaseId);
    Task<List<SpBranchTransfer>> Mushak6P5(int SaleId);
    Task<List<SpGetTdsPurchaseCertificate>> GetTdsCertificate(int purchaseId);
}