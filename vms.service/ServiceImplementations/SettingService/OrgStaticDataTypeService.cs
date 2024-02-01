using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class OrgStaticDataTypeService : ServiceBase<OrgStaticDataType>, IOrgStaticDataTypeService
{
    private readonly IOrgStaticDataTypeRepository _repository;
    public OrgStaticDataTypeService(IOrgStaticDataTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }
}