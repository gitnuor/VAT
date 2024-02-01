using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class CustomsAndVatcommissionarateService : ServiceBase<CustomsAndVatcommissionarate>, ICustomsAndVatcommissionarateService
{
    private readonly ICustomsAndVatcommissionarateRepository _repository;
    public CustomsAndVatcommissionarateService(ICustomsAndVatcommissionarateRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetCustomsAndVatcommissionarateSelectList()
    {
        var customsAndVatCommissionarates = await _repository.CustomsAndVatcommissionarates();
        return customsAndVatCommissionarates.ConvertToCustomSelectList(nameof(CustomsAndVatcommissionarate.CustomsAndVatcommissionarateId),
            nameof(CustomsAndVatcommissionarate.Name));
    }

    public async Task<IEnumerable<CustomsAndVatcommissionarate>> CustomsAndVatcommissionarates()
    {
        return await _repository.CustomsAndVatcommissionarates();
    }
}