using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.PurchaseReport;

namespace vms.service.Services.ReportService;

public interface IPurchaseReportService
{
    Task<IEnumerable<PurchaseReportViewModel>> GetPurchaseReportByBranchAndVendor(int organizationId, int branchId, int vendorId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<PurchaseReportViewModel>> GetPurchaseReportByBranch(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<PurchaseReportViewModel>> GetPurchaseReportByVendor(int organizationId, int vendorId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<PurchaseReportViewModel>> GetPurchaseReportAll(int organizationId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<ViewPurchaseDetail>> GetPuchaseDetailsListData(string organizationId);
    Task<IEnumerable<ViewPurchaseDetail>> GetPuchaseDetailsListDataByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewDebitNoteDetail>> DebitNoteDetailsReport(string organizationId);
    Task<IEnumerable<ViewDebitNoteDetail>> DebitNoteDetailsReportByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch);
}