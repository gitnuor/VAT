using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IBrandRepository : IRepositoryBase<Brand>
{
    Task<IEnumerable<Brand>> GetBrandsByOrg(string pOrgId);

}