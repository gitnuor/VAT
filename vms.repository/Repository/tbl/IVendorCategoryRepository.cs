using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl
{
	public interface IVendorCategoryRepository: IRepositoryBase<VendorCategory>
	{
		Task<IEnumerable<VendorCategory>> GetVendorCategory(string pOrgId);
	}
}
