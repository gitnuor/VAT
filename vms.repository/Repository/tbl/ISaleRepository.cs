using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.Dto.SalesCreate;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.entity.viewModels.ReportsViewModel;
using vms.entity.viewModels.VmSalesCombineParamsModels;

namespace vms.repository.Repository.tbl;

public interface ISaleRepository : IRepositoryBase<Sale>
{
    Task<bool> InsertData(vmSaleOrder saleOrder);

    Task<bool> InsertCreditNote(vmCreditNote vmCreditNote);

    Task<int> InsertSale(VmSalesCombineParamsModel saleOrder);

    Task<int> InsertSaleFromApi(SalesCombinedInsertParamDto sale, string apiData = null);

    Task<Sale> GetSalesDetailsAsync(vmSalesDetails salesDetails);

    Task<IEnumerable<Sale>> GetSalesDetails(int orgId);
    Task<IEnumerable<Sale>> GetSalesByOrganization(string orgIdEnc);
    Task<IEnumerable<Sale>> GetSalesByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewVdsSale>> GetSalesViewByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewSale>> GetSalesListByOrganization(string orgIdEnc);
    Task<IEnumerable<ViewSale>> GetSalesListByOrganizationByBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewSalesLocal>> GetSalesLocalListByOrganization(string orgIdEnc);
    Task<IEnumerable<ViewSalesExport>> GetSalesExportListByOrganization(string orgIdEnc);

    Task<Sale> GetSaleData(string idEnc);
    Task<ViewSale> GetViewSale(string idEnc);
    Task<ViewSalesLocal> GetViewSaleLocal(string idEnc);
    Task<ViewSalesExport> GetViewSaleExport(string idEnc);

    Task<IEnumerable<Sale>> GetSalesDue(int orgId);

    Task<int> InsertSaleLocalData(VmSalesCombineParamsModel sale);

    Task<bool> ProcessUploadedSimplifiedSale(long fileUploadId, int organizationId);
    Task<bool> ProcessUploadedSimplifiedLocalSaleCalculatedByVat(long fileUploadId, int organizationId);
	Task<IEnumerable<Sale>> GetSalesDueByOrganization(string orgIdEnc);
    Task<IEnumerable<Sale>> GetSalesDueByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewSalesPaymentAgingReport>> GetSalesAgingReport(string orgIdEnc);
    Task<List<SpGetProductSale>> ProductSalesListReport(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId);
}
