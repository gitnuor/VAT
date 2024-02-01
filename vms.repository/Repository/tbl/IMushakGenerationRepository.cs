using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.repository.Repository.tbl;

public interface IMushakGenerationRepository : IRepositoryBase<MushakGeneration>
{

    Task<IEnumerable<MushakGeneration>> GetMushakGenerations(int orgIdEnc);
    Task<MushakGeneration> GetMushakGeneration(string idEnc);

    Task<bool> InsertMushak(SpAddMushakReturnBasicInfo vm);
    Task<bool> InsertReturnPlanToPaymentInfo(SpAddMushakReturnPlanToPaymentInfo model);
    Task<bool> InsertReturnPaymentInfo(SpAddMushakReturnPaymentInfo model);
    Task<bool> InsertSubmissionInfo(SpAddMushakReturnSubmissionInfo vm);
    Task<bool> InsertReturnReturnedAmountInfo(SpAddMushakReturnReturnedAmountInfo vm);
    Task<bool> InsertAddMushakReturnCompleteProcess(int id);
    Task<List<SpPurchaseCalcBook>> Mushak6P1(int productId, int orgId, int orgBranchId, string fromDate, string toDate);
    Task<List<SpPurchaseSaleCalcBook>> Mushak6P2P1(int productId, int orgId, int branchId, int vendorId, int customerId, string fromDate, string toDate);
    Task<List<SpSalesCalcBook>> Mushak6P2(int productId, int orgId, int orgBranchId, string fromDate, string toDate);
    Task<List<Sp4p3>> Mushak4P3(int PriceDeclarId);
    Task<VmMushakHighValue> Mushak6P10(int organizationId, int year, int month, decimal? lowerLimitOfHighValSale);
    Task<List<SpDebitMushak>> Mushak6P8(int DebitNoteID);
    Task<List<SpSalesTaxInvoice>> Mushak6P3(int SalesId);
    Task<List<SpCreditMushak>> Mushak6P7(int CreditNoteID);
    Task<List<SpVdsPurchaseCertificate>> Mushak6P6(int PurchaseId);
    Task<List<SpBranchTransfer>> Mushak6P5(int SaleId);

    Task<List<SpGetTdsPurchaseCertificate>> GetTdsCertificate(int purchaseId);
}