using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.viewModels.PurchaseReport;

namespace vms.service.Services.ReportService;

public interface IPurchaseSummeryReportService
{
    Task<IEnumerable<PurchaseSummeryReportViewModel>> GetByBranch(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<PurchaseSummeryReportViewModel>> GetByVendor(int organizationId, int vendorId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<PurchaseSummeryReportViewModel>> GetAll(int organizationId, DateTime? fromDate, DateTime? toDate, int userId);
}