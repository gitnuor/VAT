using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IAdjustmentTypeService : IServiceBase<AdjustmentType>
{
    Task<IEnumerable<CustomSelectListItem>> GetAdjustmentTypeSelectList();
}