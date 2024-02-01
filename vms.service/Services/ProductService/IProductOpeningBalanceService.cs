using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.ProductService;

public interface IProductOpeningBalanceService : IServiceBase<ProductOpeningBalance>
{
    Task<IEnumerable<ProductOpeningBalance>> GetProductOpeningBalanceList();
}