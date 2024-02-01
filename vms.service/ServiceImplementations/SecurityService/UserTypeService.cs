using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SecurityService;

namespace vms.service.ServiceImplementations.SecurityService;

public class UserTypeService : ServiceBase<UserType>, IUserTypeService
{
    public UserTypeService(IUserTypeRepository repository) : base(repository)
    {
    }
}