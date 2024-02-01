using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IDistrictService : IServiceBase<District>
{
    Task<IEnumerable<District>> GetDistrictList();
    Task<IEnumerable<CustomSelectListItem>> DistrictSelectList();
}