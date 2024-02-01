using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.ThirdPartyService;

namespace vms.service.ServiceImplementations.ThirdPartyService;

public class CustomerDeliveryAddressService
    (ICustomerDeliveryAddressRepository repository) : ServiceBase<CustomerDeliveryAddress>(repository),
        ICustomerDeliveryAddressService;