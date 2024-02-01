using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class ProductProductTypeMappingService : ServiceBase<ProductProductTypeMapping>,
    IProductProductTypeMappingService
{
    private readonly IProductProductTypeMappingRepository _repository;

    public ProductProductTypeMappingService(IProductProductTypeMappingRepository repository) : base(repository)
    {
        _repository = repository;
    }
}