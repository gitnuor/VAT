using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using vms.entity.StoredProcedureModel.ProductReport;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl
{
	public class ProductReportRepository(DbContext context) : IProductReportRepository
	{
		public async Task<List<SpGetProductCurrentStock>> ProductCurrentStockReport(int orgId, int orgBranchId,
			int productTypeId)
		{
			var parameter = new DynamicParameters();
			parameter.Add("@OrganizationId", orgId);
			parameter.Add("@BranchId", orgBranchId);
			parameter.Add("@ProductTypeId", productTypeId);
			var result = await context.Database.GetDbConnection().QueryAsync<SpGetProductCurrentStock>(
				"SpGetProductCurrentStock @OrganizationId, @BranchId, @ProductTypeId", parameter, commandTimeout: 500);

			return result.ToList();
		}

		public async Task<List<SpGetProductStock>> ProductStockReport(int orgId, int orgBranchId, int productTypeId,
			DateTime? toDate)
		{
			var parameter = new DynamicParameters();
			parameter.Add("@OrganizationId", orgId);
			parameter.Add("@BranchId", orgBranchId);
			parameter.Add("@ProductTypeId", productTypeId);
			parameter.Add("@StockDate", toDate);
			var result = await context.Database.GetDbConnection().QueryAsync<SpGetProductStock>(
				"SpGetProductStock @OrganizationId, @BranchId, @ProductTypeId, @StockDate", parameter,
				commandTimeout: 500);

			return result.ToList();
		}
	}
}