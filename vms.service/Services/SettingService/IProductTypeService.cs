using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IProductTypeService : IServiceBase<ProductType>
{
    Task<IEnumerable<CustomSelectListItem>> GetProductTypeSelectList();
    Task<IEnumerable<CustomSelectListItem>> GetProductTypeOnlyInventorySelectList();
}