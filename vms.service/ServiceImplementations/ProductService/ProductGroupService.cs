using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class ProductGroupService : ServiceBase<ProductGroup>, IProductGroupService
{
    private readonly IProductGroupRepository _repository;

    public ProductGroupService(IProductGroupRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductGroup>> GetProductGroups()
    {
        return await _repository.GetProductGroups();
    }

    public async Task<ProductGroup> GetProductGroup(string idEnc)
    {
        return await _repository.GetProductGroup(idEnc);
    }

	public async Task<ProductGroup> GetProductGroupByName(int organizationId, string groupName)
	{
		return await _repository.GetProductGroupByName(organizationId, groupName);
	}

	public async Task<IEnumerable<ProductGroup>> GetProductGroupsByOrg(string pOrgId)
    {
        return await _repository.GetProductGroupsByOrg(pOrgId);
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetProductGroupSelectList(string orgId)
    {
        var categories = await _repository.GetProductGroupsByOrg(orgId);
        return categories.ConvertToCustomSelectList(nameof(ProductGroup.ProductGroupId),
            nameof(ProductGroup.Name));
    }
}