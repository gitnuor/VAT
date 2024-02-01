using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IBusinessCategoryService : IServiceBase<BusinessCategory>
{
    Task<IEnumerable<CustomSelectListItem>> GetAllCategorySelectListItem();
    Task<IEnumerable<CustomSelectListItem>> BusinessCategorySelectList();
}