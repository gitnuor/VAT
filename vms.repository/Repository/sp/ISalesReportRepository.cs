using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel.Sales;

namespace vms.repository.Repository.sp;

public interface ISalesReportRepository
{
    Task<IEnumerable<SpGetReportSalesModel>> GetSalesReportData(int organizationId, int branchId, int customerId,
        DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<SpGetReportSummerySalesModel>> GetSalesSummeryReportData(int organizationId, int branchId, int customerId,
        DateTime? fromDate, DateTime? toDate, int userId);
	Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsListData(string organizationId);
    Task<IEnumerable<ViewSalesDetail>> GetSalesDetailsListDataByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch);

    Task<IEnumerable<ViewCreditNoteDetail>> CreditNoteDetailsReport(string organizationId);
    Task<IEnumerable<ViewCreditNoteDetail>> CreditNoteDetailsReportByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch);
}