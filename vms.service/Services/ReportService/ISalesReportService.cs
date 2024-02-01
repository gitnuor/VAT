using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.SalesReport;

namespace vms.service.Services.ReportService;

public interface ISalesReportService
{
    Task<IEnumerable<SalesReportViewModel>> GetSalesReportByBranchAndCustomer(int organizationId, int branchId, int customerId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<SalesReportViewModel>> GetSalesReportByBranch(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<SalesReportViewModel>> GetSalesReportByCustomer(int organizationId, int customerId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<SalesReportViewModel>> GetSalesReportAll(int organizationId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsListData(string organizationId);
    Task<IEnumerable<ViewCreditNoteDetail>> CreditNoteDetailsReport(string organizationId);
    Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsListDataByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewCreditNoteDetail>> CreditNoteDetailsReportByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch);
}