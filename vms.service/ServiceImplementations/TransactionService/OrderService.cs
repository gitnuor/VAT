using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class OrderService : ServiceBase<Order>, IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository) : base(repository)
    {
        _repository = repository;
    }
}