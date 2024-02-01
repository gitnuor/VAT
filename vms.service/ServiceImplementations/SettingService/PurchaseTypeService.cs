using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class PurchaseTypeService : ServiceBase<PurchaseType>, IPurchaseTypeService
{
    private readonly IPurchaseTypeRepository _repository;

    public PurchaseTypeService(IPurchaseTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }
}