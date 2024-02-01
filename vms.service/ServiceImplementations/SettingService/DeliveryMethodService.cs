using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class DeliveryMethodService : ServiceBase<DeliveryMethod>, IDeliveryMethodService
{
    private readonly IDeliveryMethodRepository _repository;

    public DeliveryMethodService(IDeliveryMethodRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DeliveryMethod>> GetDeliveryMethods()
    {
        return await _repository.GetDeliveryMethods();
    }

    public async Task<DeliveryMethod> GetDeliveryMethod(string idEnc)
    {
        return await _repository.GetDeliveryMethod(idEnc);
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetDeliveryMethodSelectList()
    {
        var deliveryMethodList = await _repository.GetDeliveryMethods();
        return deliveryMethodList.ConvertToCustomSelectList(nameof(DeliveryMethod.DeliveryMethodId),
            nameof(DeliveryMethod.Name));
    }
}