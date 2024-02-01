using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.BillingService;
using vms.service.Services.ThirdPartyService;

namespace vms.service.ServiceImplementations.BillingService;

public class SubscriptionBillDetailService
	(ISubscriptionBillDetailRepository repository) : ServiceBase<SubscriptionBillDetail>(repository),
		ISubscriptionBillDetailService;