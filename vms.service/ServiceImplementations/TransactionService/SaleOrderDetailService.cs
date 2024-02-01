using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class SaleOrderDetailService : ServiceBase<SalesDetail>, ISaleOrderDetailService
{
    private readonly ISaleOrderDetailsRepository _repository;

    public SaleOrderDetailService(ISaleOrderDetailsRepository repository) : base(repository)
    {
        _repository = repository;
    }
}