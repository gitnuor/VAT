using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.ProductService;

public interface IProductGroupService : IServiceBase<ProductGroup>
{
    Task<IEnumerable<ProductGroup>> GetProductGroups();
    Task<ProductGroup> GetProductGroup(string idEnc);
    Task<IEnumerable<ProductGroup>> GetProductGroupsByOrg(string pOrgid);
    Task<IEnumerable<CustomSelectListItem>> GetProductGroupSelectList(string pOrgid);
    Task<ProductGroup> GetProductGroupByName(int organizationId, string groupName);
}