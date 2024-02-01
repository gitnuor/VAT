using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.SubscriptionAndBilling;

namespace vms.service.Services.BillingService;

public interface ISubscriptionBillService : IServiceBase<SubscriptionBill>
{
	Task<IEnumerable<ViewSubscriptionBill>> GetSubscriptionBill(int orgId); 
	Task<int> GenerateSubscriptionBill(SubscriptionBillCreateViewModel model, int organizationId, int createdBy, DateTime createdTime);
}