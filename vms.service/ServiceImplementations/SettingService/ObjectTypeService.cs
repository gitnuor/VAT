using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class ObjectTypeService : ServiceBase<ObjectType>, IObjectTypeService
{
    private readonly IObjectTypeRepository _repository;
    public ObjectTypeService(IObjectTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }
}