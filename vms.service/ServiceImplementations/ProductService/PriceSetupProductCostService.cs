using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class PriceSetupProductCostService : ServiceBase<PriceSetupProductCost>, IPriceSetupProductCostService
{
    private readonly IPriceSetupProductCostRepository _repository;

    public PriceSetupProductCostService(IPriceSetupProductCostRepository repository) : base(repository)
    {
        _repository = repository;
    }
}