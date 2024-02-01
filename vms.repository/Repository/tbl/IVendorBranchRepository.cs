using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IVendorBranchRepository : IRepositoryBase<VendorBranch>
{
	Task<IEnumerable<ViewVendorBranch>> GetVendorBranchesByVendor(int vendorId);
}