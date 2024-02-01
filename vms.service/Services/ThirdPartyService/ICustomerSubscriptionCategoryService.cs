using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.ThirdPartyService
{
	public interface ICustomerSubscriptionCategoryService : IServiceBase<CustomerSubscriptionCategory>
	{
		Task<IEnumerable<CustomerSubscriptionCategory>> GetCustomerCategory(string pOrgId);
    }
}
