using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.SecurityService;

public interface IRoleRightService : IServiceBase<RoleRight>
{
    Task<IEnumerable<RoleRight>> GetByRole(int roleId);
    void DeleteObjectList(List<RoleRight> role);
    void InsertObjectList(List<RoleRight> role);
}