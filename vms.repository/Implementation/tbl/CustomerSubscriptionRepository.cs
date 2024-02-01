using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class CustomerSubscriptionRepository(DbContext context) : RepositoryBase<CustomerSubscription>(context),
	ICustomerSubscriptionRepository
{
	public async Task<IEnumerable<ViewCustomerSubscription>> GetCustomerSubscription(int orgId)
	{
		return await context.Set<ViewCustomerSubscription>()
			.Where(s => s.OrganizationId == orgId)
			.AsNoTracking()
			.ToListAsync();
	}

	public Task<IEnumerable<SpGetUnBilledSubscriptions>> GetUnBilledSubscription(int organizationId,
		int billingOfficeId,
		int collectionOfficeId,
		int year,
		int month)
	{
		var parameter = new DynamicParameters();
		parameter.Add("@Year", year);
		parameter.Add("@Month", month);
		parameter.Add("@BillingOfficeId", billingOfficeId);
		parameter.Add("@CollectionOfficeId", collectionOfficeId);
		parameter.Add("@OrganizationId", organizationId);
		return context.Database
			.GetDbConnection()
			.QueryAsync<SpGetUnBilledSubscriptions>(
				"EXEC [dbo].[SpGetUnBilledSubscriptions] @Year, @Month, @BillingOfficeId, @CollectionOfficeId, @OrganizationId",
				parameter,
				commandTimeout: 500);
	}
}