using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class ProductVatService : ServiceBase<ProductVat>, IProductVatService
{
    private readonly IProductVatRepository _repository;

    public ProductVatService(IProductVatRepository repository) : base(repository)
    {
        _repository = repository;
    }
}