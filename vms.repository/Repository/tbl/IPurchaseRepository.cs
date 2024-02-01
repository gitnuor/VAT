using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.StoredProcedureModel.ParamModel;
using vms.entity.viewModels;

namespace vms.repository.Repository.tbl;

public interface IPurchaseRepository : IRepositoryBase<Purchase>
{
    Task<bool> InsertDebitNote(vmDebitNote vmDebitNote);

    Task<bool> ManagePurchaseDue(vmPurchasePayment vmPurchase);

    Task<Purchase> GetPurchaseDetails(string pEncryptedId);

    Task<IEnumerable<Purchase>> GetPurchaseDue(int orgId);

    Task<bool> InsertTransferReceive(vmTransferReceive vm);

    Task<IEnumerable<Purchase>> GetPurchases(int pOrgId);
    Task<IEnumerable<Purchase>> GetPurchaseByOrganization(string orgIdEnc);
    Task<IEnumerable<ViewPurchase>> GetPurchaseListByOrganization(string orgIdEnc);
    Task<IEnumerable<ViewPurchase>> GetPurchaseListByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewPurchaseLocal>> GetPurchaseLocalListByOrganization(string orgIdEnc);
    Task<IEnumerable<ViewPurchaseLocal>> GetPurchaseLocalListByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewPurchaseImport>> GetPurchaseImportListByOrganization(string orgIdEnc);
		Task<IEnumerable<ViewVdsPurchase>> GetVdsPurchaseListByOrganization(string orgIdEnc);
    Task<IEnumerable<ViewVdsPurchase>> GetVdsPurchaseListByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<Purchase>> GetVdsPurchases(int pOrgId);
    Task<IEnumerable<Purchase>> GetVdsPurchasesWithDueVdsPayment(int pOrgId);
    Task<IEnumerable<Purchase>> GetVdsPurchasesWithVdsPayment(string orgIdEnc);
    Task<IEnumerable<Purchase>> GetVdsPurchasesWithVdsPaymentAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<Purchase>> GetTdsPurchasesWithDueTdsPayment(int pOrgId);
    Task<IEnumerable<Purchase>> GetTdsPurchasesWithTdsPayment(string orgIdEnc);
    Task<int> InsertPurchase(SpInsertPurchaseCombinedParam purchase);
    Task<int> InsertApiPurchase(SpInsertPurchaseFromApiCombinedParam purchase, string apiData = null);

    Task<List<SpMonthlyPurchaseReport>> MonthlyPurchaseReport(int userId,int PurReason,  int organizationId, int vendorId, string invoiceNo, DateTime? fromDate, DateTime? toDate);
    Task<List<SpGetPurchaseReportByProduct>> GetPurchaseReportByProduct(int purReason, int organizationId, int productId, string invoiceNo, DateTime? fromDate, DateTime? toDate, int userId);
    Task<List<SpGetProductPurchase>> ProductPurchaseListReport(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate);

    Task<bool> ProcessUploadedSimplifiedPurchase(long fileUploadId, int organizationId);
    Task<bool> ProcessUploadedSimplifiedLocalPurchase(long fileUploadId, int organizationId);
    Task<List<ViewPurchaseLocal>> GetPurchaseVdsList(int organizationId, DateTime? fromDate, DateTime? toDate);
    Task<List<ViewPurchaseLocal>> GetPurchaseVdsListByOrgAndBranch(int organizationId, DateTime? fromDate, DateTime? toDate, List<int> branchIds, bool isRequiredBranch);
}