using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.SettingService;

public interface IOrganizationService : IServiceBase<Organization>
{
    Task<IEnumerable<Organization>> GetOrganizations(int orgIdEnc);
    Task<Organization> GetOrganization(string idEnc);
    Task<IEnumerable<Organization>> GetParentOrganizations(int par_orgId);
}