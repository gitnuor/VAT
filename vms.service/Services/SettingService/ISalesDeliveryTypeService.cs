using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.SettingService;

public interface ISalesDeliveryTypeService : IServiceBase<SalesDeliveryType>
{
    Task<IEnumerable<SalesDeliveryType>> GetSalesDeliveryTypes();
    Task<SalesDeliveryType> GetSalesDeliveryType(string idEnc);
}