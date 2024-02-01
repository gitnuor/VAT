using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class ProductOpeningBalanceService : ServiceBase<ProductOpeningBalance>, IProductOpeningBalanceService
{
    private readonly IProductOpeningBalanceRepository _repository;
    public ProductOpeningBalanceService(IProductOpeningBalanceRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductOpeningBalance>> GetProductOpeningBalanceList()
    {
        return await _repository.Query().SelectAsync();
    }
}