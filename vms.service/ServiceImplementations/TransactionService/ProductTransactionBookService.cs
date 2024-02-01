using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class ProductTransactionBookService : ServiceBase<ProductTransactionBook>, IProductTransactionBookService
{
    private readonly IProductTransactionBookRepository _repository;

    public ProductTransactionBookService(IProductTransactionBookRepository repository) : base(repository)
    {
        _repository = repository;
    }
}