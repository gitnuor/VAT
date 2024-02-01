using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.viewModels.SalesReport;

namespace vms.service.Services.ReportService;

public interface ISalesSummeryReportService
{
    Task<IEnumerable<SalesSummeryReportViewModel>> GetByBranch(int organizationId, int branchId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<SalesSummeryReportViewModel>> GetByCustomer(int organizationId, int customerId, DateTime? fromDate, DateTime? toDate, int userId);
    Task<IEnumerable<SalesSummeryReportViewModel>> GetAll(int organizationId, DateTime? fromDate, DateTime? toDate, int userId);
}