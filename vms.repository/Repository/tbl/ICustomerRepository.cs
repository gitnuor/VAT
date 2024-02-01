using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface ICustomerRepository : IRepositoryBase<Customer>
{
	Task<IEnumerable<Customer>> GetCustomers(int orgIdEnc);
	Task<IEnumerable<Customer>> GetCustomerByExportType(int orgIdEnc, int exportTypeId);
	Task<Customer> GetCustomer(string idEnc);

	Task<IEnumerable<Customer>> GetAllCustomersByOrg(string pOrgId);
	IQueryable<ViewCustomer> GetAllCustomer();
	Task<IEnumerable<ViewCustomer>> GetCustomerListByOrg(string pOrgId);
	Task<IEnumerable<ViewCustomerLocal>> GetCustomerLocalListByOrg(string pOrgId);
	Task<IEnumerable<ViewCustomerForeign>> GetCustomerForeignListByOrg(string pOrgId);
	Task<IEnumerable<ViewCustomerWithBranch>> GetCustomerWithBranchListByOrg(string pOrgId);
	Task<IEnumerable<ViewCustomerWithBranch>> GetCustomerWithBranchListByBranch(int branchId);
	Task<IEnumerable<Customer>> GetLocalCustomersByOrg(string pOrgId);
	Task<IEnumerable<Customer>> GetForeignCustomersByOrg(string pOrgId);
	Task<Customer> GetByReference(string refKey, int orgId);
    Task<ViewCustomerLocal> GetLocalCustomersById(int id);
}