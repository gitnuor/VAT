using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.Dto.Sales;
using vms.entity.Dto.SalesExport;
using vms.entity.Dto.SalesLocal;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.viewModels;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.service.Services.TransactionService;

public interface ISaleService : IServiceBase<Sale>
{
    Task<bool> InsertData(vmSaleOrder saleOrder);
    Task<bool> InsertCreditNote(vmCreditNote vmCreditNote);
    Task<bool> InsertCreditNote(VmSalesCreditNotePost vmCreditNote);
    Task<IList<Sale>> GetSalesDetailsAsync(vmSalesDetails salesDetails);
    Task<IEnumerable<Sale>> GetSalesDetails(int orgId);
    Task<IEnumerable<Sale>> GetSalesByOrganization(string orgIdEnc);
    Task<IEnumerable<Sale>> GetSalesByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewVdsSale>> GetSalesViewByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewSale>> GetSalesListByOrganization(string orgIdEnc);
    Task<IEnumerable<ViewSale>> GetSalesListByOrganizationByBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewSalesLocal>> GetSalesLocalListByOrganization(string orgIdEnc);
    Task<IEnumerable<ViewSalesExport>> GetSalesExportListByOrganization(string orgIdEnc);
    Task<IEnumerable<Sale>> GetSalesDueByOrganization(string orgIdEnc);
    Task<IEnumerable<Sale>> GetSalesDueByOrganizationAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch);
    Task<Sale> GetSaleData(string idEnc);
    Task<IEnumerable<Sale>> GetSalesDue(int orgId);
    Task<int> InsertLocalSale(VmSaleLocalPost vmSaleLocalPost);
    Task<int> InsertLocalSaleWithBreakdown(VmSaleLocalPostWithBreakdown vmSaleLocalPost);
    Task<int> InsertLocalSale(SalesLocalPostDto sales, string apiData = null, string token = null);
    Task<int> InsertApiSale(SalesCombinedPostDto sales, string apiData = null, string token = null);

    Task<int> InsertLocalSaleExport(VmSaleExportPost vmSaleExportPost);
    Task<int> InsertLocalSaleDraft(VmSaleLocalPost vmSaleLocalPost);

    Task<int> InsertLocalSaleExportDraft(VmSaleExportPost vmSaleExportPost);
    Task<bool> ProcessUploadedSimplifiedSale(long fileUploadId, int organizationId);
    Task<bool> ProcessUploadedSimplifiedLocalSaleCalculatedByVat(long fileUploadId, int organizationId);
    Task<Sale> Approve(SaleApproveOrRejectViewModel model);
    Task<Sale> Reject(SaleApproveOrRejectViewModel model);

    Task<ViewSale> GetViewSale(string idEnc);
    Task<ViewSalesLocal> GetViewSaleLocal(string idEnc);
    Task<ViewSalesExport> GetViewSaleExport(string idEnc);

    //for api
    Task<IEnumerable<SaleDto>> GetSalesDtoListByOrganization(string orgIdEnc);
    Task<SaleWithDetailDto> GetSalesDto(string idEnc);
    Task<IEnumerable<SalesLocalDto>> GetSalesLocalDtoListByOrganization(string orgIdEnc);
    Task<SalesLocalWithDetailDto> GetSalesLocalDto(string idEnc);
    Task<IEnumerable<SalesExportDto>> GetSalesExportDtoListByOrganization(string orgIdEnc);
    Task<SalesExportWithDetailDto> GetSalesExportDto(string idEnc);
    Task<IEnumerable<ViewSalesPaymentAgingReport>> GetSalesAgingReport(string orgIdEnc);
    Task<List<SpGetProductSale>> ProductSalesListReport(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId);
}