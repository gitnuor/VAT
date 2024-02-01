using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IProductVatTypeRepository : IRepositoryBase<ProductVattype>
{
    Task<IEnumerable<ProductVattype>> GetProductVattypes();

    Task<ProductVattype> GetProductVattype(string idEnc);
}