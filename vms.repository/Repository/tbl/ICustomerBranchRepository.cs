using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface ICustomerBranchRepository : IRepositoryBase<CustomerBranch>
{
	Task<IEnumerable<ViewCustomerBranch>> GetCustomerBranchesByCustomer(int customerId);
}