using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SecurityService;
using vms.entity.Utility;

namespace vms.service.ServiceImplementations.SecurityService;

public class RoleService : ServiceBase<Role>, IRoleService
{
    private readonly IRoleRepository _repository;

    public RoleService(IRoleRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Role>> GetRoles(int orgIdEnc)
    {
        return await _repository.GetRoles(orgIdEnc);
    }

    public async Task<Role> GetRole(string idEnc)
    {
        return await _repository.GetRole(idEnc);
    }

	public async Task<Role> GetRoleByName(int organizationId, string roleName)
	{
		return await _repository.GetRoleByName(organizationId, roleName);
	}

    public async Task<IEnumerable<CustomSelectListItem>> GetRoleSelectList(int orgId)
    {
        var roles = await _repository.GetRoles(orgId);
        return roles.ConvertToCustomSelectList(nameof(Role.RoleId),
            nameof(Role.RoleName));
    }
}