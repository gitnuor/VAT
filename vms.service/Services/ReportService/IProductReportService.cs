using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel.HTMLMushak;
using vms.entity.StoredProcedureModel.ProductReport;

namespace vms.service.Services.ReportService
{
    public interface IProductReportService
    {
        Task<List<SpGetProductCurrentStock>> ProductCurrentStockReport(int orgId, int orgBranchId, int productTypeId);
		Task<List<SpGetProductStock>> ProductStockReport(int orgId, int orgBranchId, int productTypeId, DateTime? toDate);

	}
}
