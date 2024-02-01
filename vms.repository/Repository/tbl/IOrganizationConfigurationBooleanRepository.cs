using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl
{
    public interface IOrganizationConfigurationBooleanRepository : IRepositoryBase<ViewOrganizationConfigurationBoolean>
	{
		Task<IEnumerable<ViewOrganizationConfigurationBoolean>> GetOrganizationConfigurationBoolean(string orgIdEnc);
	}
}
