using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface ICustomsAndVatcommissionarateService : IServiceBase<CustomsAndVatcommissionarate>
{
    Task<IEnumerable<CustomSelectListItem>> GetCustomsAndVatcommissionarateSelectList();
    Task<IEnumerable<CustomsAndVatcommissionarate>> CustomsAndVatcommissionarates();
}