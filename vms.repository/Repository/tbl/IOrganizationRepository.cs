using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IOrganizationRepository : IRepositoryBase<Organization>
{
    Task<IEnumerable<Organization>> GetOrganizations(int orgIdEnc);
    Task<Organization> GetOrganization(string idEnc);
    Task<IEnumerable<Organization>> GetParentOrganizations(int par_orgId);

}