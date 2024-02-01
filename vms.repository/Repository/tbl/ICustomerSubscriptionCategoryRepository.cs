using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl
{
	 public interface ICustomerSubscriptionCategoryRepository : IRepositoryBase<CustomerSubscriptionCategory>
	{
		Task<IEnumerable<CustomerSubscriptionCategory>> GetCustomerCategory(string pOrgId);
	}
}
