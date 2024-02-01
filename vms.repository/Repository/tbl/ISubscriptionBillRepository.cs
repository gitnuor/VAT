using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels.SubscriptionAndBilling;

namespace vms.repository.Repository.tbl;

public interface ISubscriptionBillRepository : IRepositoryBase<SubscriptionBill>
{
	Task<IEnumerable<ViewSubscriptionBill>> GetSubscriptionBill(int orgId);
	Task<int> GenerateSubscriptionBill(SubscriptionBillCreateViewModel model, int organizationId, int createdBy, DateTime createdTime);
}