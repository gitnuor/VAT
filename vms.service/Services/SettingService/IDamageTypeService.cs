using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IDamageTypeService : IServiceBase<DamageType>
{
    Task<IEnumerable<DamageType>> GetDamageTypes(int orgIdEnc);
    Task<DamageType> GetDamageType(string idEnc);
    Task<IEnumerable<CustomSelectListItem>> GetDamageTypeSelectList(int orgId);
}