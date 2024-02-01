using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class PurchaseImportTariffService : ServiceBase<PurchaseImportTariff>, IPurchaseImportTariffService
{
    private readonly IPurchaseImportTariffRepository _repository;
    public PurchaseImportTariffService(IPurchaseImportTariffRepository repository) : base(repository)
    {
        _repository = repository;
    }
}