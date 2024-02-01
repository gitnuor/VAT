using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.ThirdPartyService
{
	public interface  ICustomerCategoryService: IServiceBase<CustomerCategory>
	{
		Task<IEnumerable<CustomerCategory>> GetCustomerCategory(string pOrgId);
        //Task<CustomerCategory> GetCategory(string idEnc);
    }
}
