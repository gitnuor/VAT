using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class ProductTypeService : ServiceBase<ProductType>, IProductTypeService
{
    private readonly IProductTypeRepository _repository;

    public ProductTypeService(IProductTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductType>> GetProductTypes()
    {
        return await _repository.GetProductTypes();
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetProductTypeSelectList()
    {
        var types = await _repository.GetProductTypes();
        return types.ConvertToCustomSelectList(nameof(ProductType.ProductTypeId),
            nameof(ProductType.Name));
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetProductTypeOnlyInventorySelectList()
    {
		var types = await _repository.GetProductTypes();
		return types.Where(t => t.IsInventory).ConvertToCustomSelectList(nameof(ProductType.ProductTypeId),
			nameof(ProductType.Name));
	}
}