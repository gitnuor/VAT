using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SecurityService;

namespace vms.service.ServiceImplementations.SecurityService;

public class RoleRightService : ServiceBase<RoleRight>, IRoleRightService
{
    private readonly IRoleRightRepository _repository;

    public RoleRightService(IRoleRightRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<RoleRight>> GetByRole(int roleId)
    {
        return _repository.GetByRole(roleId);
    }

    public void DeleteObjectList(List<RoleRight> role)
    {
        _repository.DeleteObjectList(role);
    }

    public void InsertObjectList(List<RoleRight> role)
    {
        _repository.InsertObjectList(role);
    }
}