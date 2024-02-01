using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.ThirdPartyService;

namespace vms.service.ServiceImplementations.ThirdPartyService;

public class CustomerSubscriptionDetailService
    (ICustomerSubscriptionDetailRepository repository) : ServiceBase<CustomerSubscriptionDetail>(repository),
        ICustomerSubscriptionDetailService;