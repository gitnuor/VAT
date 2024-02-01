using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SecurityService;

public interface IRoleService : IServiceBase<Role>
{
    Task<IEnumerable<Role>> GetRoles(int orgIdEnc);
    Task<Role> GetRole(string idEnc);
    Task<Role> GetRoleByName(int organizationId, string roleName);
    Task<IEnumerable<CustomSelectListItem>> GetRoleSelectList(int orgId);
}