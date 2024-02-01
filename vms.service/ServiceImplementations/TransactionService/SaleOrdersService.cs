using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class SaleOrdersService : ServiceBase<SaleOrders>, ISaleOrdersService
{
    private readonly ISaleOrderRepository _repository;

    public SaleOrdersService(ISaleOrderRepository repository) : base(repository)
    {
        _repository = repository;
    }
}