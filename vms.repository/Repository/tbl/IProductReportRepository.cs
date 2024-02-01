using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.StoredProcedureModel.ProductReport;

namespace vms.repository.Repository.tbl
{
	public interface IProductReportRepository
	{
		Task<List<SpGetProductCurrentStock>> ProductCurrentStockReport(int orgId, int orgBranchId, int productTypeId);
		Task<List<SpGetProductStock>> ProductStockReport(int orgId, int orgBranchId, int productTypeId, DateTime? toDate);
	}
}
