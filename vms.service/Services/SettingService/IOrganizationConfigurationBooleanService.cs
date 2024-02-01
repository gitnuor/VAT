using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService
{
    public interface IOrganizationConfigurationBooleanService : IServiceBase<ViewOrganizationConfigurationBoolean>
    {
        Task<IEnumerable<ViewOrganizationConfigurationBoolean>> GetOrganizationConfigurationBoolean(string orgIdEnc);
    }
}
