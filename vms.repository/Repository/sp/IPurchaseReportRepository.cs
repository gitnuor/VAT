using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.StoredProcedureModel.Purchase;

namespace vms.repository.Repository.sp;

public interface IPurchaseReportRepository
{
    Task<IEnumerable<SpGetReportPurchaseModel>> GetPurchaseReportData(int organizationId, int branchId, int vendorId,
        DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<SpGetReportSummeryPurchaseModel>> GetPurchaseSummeryReportData(int organizationId, int branchId, int vendorId,
        DateTime? fromDate, DateTime? toDate, int userId);
	
		Task<IEnumerable<ViewPurchaseDetail>> GetPuchaseDetailsListData(string organizationId);
    Task<IEnumerable<ViewPurchaseDetail>> GetPuchaseDetailsListDataByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch);
    Task<IEnumerable<ViewDebitNoteDetail>> DebitNoteDetailsReport(string organizationId);
    Task<IEnumerable<ViewDebitNoteDetail>> DebitNoteDetailsReportByOrgAndBranch(string organizationId, List<int> branchIds, bool isRequiredBranch);

}