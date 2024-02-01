using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IProductCategoryRepository : IRepositoryBase<ProductCategory>
{
    Task<IEnumerable<ProductCategory>> GetPCategories(int orgIdEnc);
    Task<ProductCategory> GetPCat(string idEnc);
	Task<IEnumerable<ProductCategory>> GetPCategoriesByOrg(string orgIdEnc);
	Task<ProductCategory> GetPCatByName(int organizationId, string categoryName);
}