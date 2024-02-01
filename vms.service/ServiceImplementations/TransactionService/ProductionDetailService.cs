using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class ProductionDetailService : ServiceBase<ProductionDetail>, IProductionDetailService
{
    private readonly IProductionDetailRepository _repository;
    public ProductionDetailService(IProductionDetailRepository repository) : base(repository)
    {
        _repository = repository;
    }
}