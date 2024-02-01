using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.ProductService;

public interface IProductCategoryService : IServiceBase<ProductCategory>
{
    Task<IEnumerable<ProductCategory>> GetPCategories(int orgIdEnc);
    Task<ProductCategory> GetPCat(string idEnc);
	Task<ProductCategory> GetPCatByName(int organizationId, string categoryName);
	Task<IEnumerable<CustomSelectListItem>> GetProductCategorySelectList(string orgId);
}