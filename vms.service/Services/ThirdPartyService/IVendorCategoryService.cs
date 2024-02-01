using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.ThirdPartyService
{
	public interface IVendorCategoryService : IServiceBase<VendorCategory>
	{
		Task<IEnumerable<VendorCategory>> GetVendorCategory(string pOrgId);
	}
}
