using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class SalesTypeService : ServiceBase<SalesType>, ISalesTypeService
{
    private readonly ISalesTypeRepository _repository;

    public SalesTypeService(ISalesTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }
}