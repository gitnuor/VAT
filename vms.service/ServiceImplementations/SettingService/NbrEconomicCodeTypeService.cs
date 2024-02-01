using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class NbrEconomicCodeTypeService : ServiceBase<NbrEconomicCodeType>, INbrEconomicCodeTypeService
{
    private readonly INbrEconomicCodeTypeRepository _repository;

    public NbrEconomicCodeTypeService(INbrEconomicCodeTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }
}