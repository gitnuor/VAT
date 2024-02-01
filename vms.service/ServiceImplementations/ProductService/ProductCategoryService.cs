using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class ProductCategoryService : ServiceBase<ProductCategory>, IProductCategoryService
{
    private readonly IProductCategoryRepository _repository;

    public ProductCategoryService(IProductCategoryRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductCategory>> GetPCategories(int orgIdEnc)
    {
        return await _repository.GetPCategories(orgIdEnc);
    }

    public async Task<ProductCategory> GetPCat(string idEnc)
    {
        return await _repository.GetPCat(idEnc);
    }

	public async Task<ProductCategory> GetPCatByName(int organizationId, string categoryName)
	{
		return await _repository.GetPCatByName(organizationId, categoryName);
	}

	public async Task<IEnumerable<CustomSelectListItem>> GetProductCategorySelectList(string orgId)
    {
        var categories = await _repository.GetPCategoriesByOrg(orgId);
        return categories.ConvertToCustomSelectList(nameof(ProductCategory.ProductCategoryId),
            nameof(ProductCategory.Name));
    }
}