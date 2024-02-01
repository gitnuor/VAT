using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using URF.Core.Abstractions;
using vms.entity.models;
using vms.entity.viewModels.SubscriptionAndBilling;
using vms.repository.Repository.tbl;
using vms.service.Services.BillingService;
using vms.service.Services.ThirdPartyService;

namespace vms.service.ServiceImplementations.BillingService;

public class SubscriptionBillService
	(ISubscriptionBillRepository repository, ISubscriptionBillDetailRepository detailRepository, IUnitOfWork unitOfWork) : ServiceBase<SubscriptionBill>(repository),
		ISubscriptionBillService
{
	public Task<IEnumerable<ViewSubscriptionBill>> GetSubscriptionBill(int orgId)
	{
		return repository.GetSubscriptionBill(orgId);
	}

	public Task<int> GenerateSubscriptionBill(SubscriptionBillCreateViewModel model, int organizationId, int createdBy,
		DateTime createdTime)
	{
		return repository.GenerateSubscriptionBill(model, organizationId, createdBy, createdTime);
	}
}