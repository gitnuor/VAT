using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IProductTypeRepository : IRepositoryBase<ProductType>
{
    Task<IEnumerable<ProductType>> GetProductTypes();
}