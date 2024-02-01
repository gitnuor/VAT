using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using vms.entity.models;
using vms.entity.viewModels.SubscriptionAndBilling;
using vms.repository.Repository.tbl;
using vms.utility;

namespace vms.repository.Implementation.tbl;

public class SubscriptionBillRepository(DbContext context) : RepositoryBase<SubscriptionBill>(context),
	ISubscriptionBillRepository
{
	public async Task<IEnumerable<ViewSubscriptionBill>> GetSubscriptionBill(int orgId)
	{
		return await context.Set<ViewSubscriptionBill>()
			.Where(s => s.OrganizationId == orgId)
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<int> GenerateSubscriptionBill(SubscriptionBillCreateViewModel model, int organizationId, int createdBy, DateTime createdTime)
	{
		try
		{
			const string sql = "EXEC [dbo].[SpGenerateBill]  " +
			                   "@OrganizationId," +
			                   "@BillingOfficeId," +
			                   "@CollectionOfficeId," +
			                   "@BillYear," +
			                   "@BillMonth," +
			                   "@CreatedBy," +
			                   "@CreatedTime," +
			                   "@DetailsJson ";

			var detailsJson = JsonConvert.SerializeObject(model.SubscriptionBillDetails);

			var parameter = new DynamicParameters();

			parameter.Add("@OrganizationId", organizationId);
			parameter.Add("@BillingOfficeId", model.BillingOfficeId);
			parameter.Add("@CollectionOfficeId", model.CollectionOfficeId);
			parameter.Add("@BillYear", model.BillYear);
			parameter.Add("@BillMonth", model.BillMonth);
			parameter.Add("@CreatedBy", createdBy);
			parameter.Add("@CreatedTime", StringGenerator.DateTimeToSqlCompatibleStringWithoutDbNull(createdTime));
			parameter.Add("@DetailsJson", detailsJson);
			await using var queryMultiple = await context.Database.GetDbConnection()
				.QueryMultipleAsync(sql, parameter, commandTimeout: 500);
			return await queryMultiple.ReadFirstAsync<int>();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}