using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class SalesDeliveryTypeService : ServiceBase<SalesDeliveryType>, ISalesDeliveryTypeService
{
    private readonly ISalesDeliveryTypeRepository _repository;

    public SalesDeliveryTypeService(ISalesDeliveryTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SalesDeliveryType>> GetSalesDeliveryTypes()
    {
        return await _repository.GetSalesDeliveryTypes();
    }

    public async Task<SalesDeliveryType> GetSalesDeliveryType(string idEnc)
    {
        return await _repository.GetSalesDeliveryType(idEnc);
    }
}