using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IRoleRepository : IRepositoryBase<Role>
{
    Task<IEnumerable<Role>> GetRoles(int orgIdEnc);
    Task<Role> GetRole(string idEnc);
    Task<Role> GetRoleByName(int organizationId, string roleName);
}