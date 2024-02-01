using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class OrgStaticDataService : ServiceBase<OrgStaticDatum>, IOrgStaticDataService
{
    private readonly IOrgStaticDataRepository _repository;
    public OrgStaticDataService(IOrgStaticDataRepository repository) : base(repository)
    {
        _repository = repository;
    }
}