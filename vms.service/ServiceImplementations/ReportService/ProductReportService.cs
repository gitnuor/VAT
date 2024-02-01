using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel.ProductReport;
using vms.repository.Repository.tbl;
using vms.service.Services.ReportService;

namespace vms.service.ServiceImplementations.ReportService
{
    public class ProductReportService(IProductReportRepository repository) : IProductReportService
    {
	    public async Task<List<SpGetProductCurrentStock>> ProductCurrentStockReport(int orgId, int orgBranchId, int productTypeId)
        {
            return await repository.ProductCurrentStockReport(orgId, orgBranchId, productTypeId);
        }

		public async Task<List<SpGetProductStock>> ProductStockReport(int orgId, int orgBranchId, int productTypeId, DateTime? toDate)
		{
			return await repository.ProductStockReport(orgId, orgBranchId, productTypeId, toDate);
		}
	}
}