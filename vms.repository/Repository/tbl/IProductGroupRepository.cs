using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IProductGroupRepository : IRepositoryBase<ProductGroup>
{
    Task<IEnumerable<ProductGroup>> GetProductGroups();
    Task<ProductGroup> GetProductGroup(string idEnc);
	Task<IEnumerable<ProductGroup>> GetProductGroupsByOrg(string pOrgId);
	Task<ProductGroup> GetProductGroupByName(int organizationId, string groupName);
}