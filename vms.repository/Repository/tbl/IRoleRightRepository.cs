using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IRoleRightRepository : IRepositoryBase<RoleRight>
{
    void DeleteObjectList(List<RoleRight> role);
    void InsertObjectList(List<RoleRight> role);
    Task<IEnumerable<RoleRight>> GetByRole(int roleId);
}