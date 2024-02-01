using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class VendorBranchRepository : RepositoryBase<VendorBranch>, IVendorBranchRepository
{
	private readonly DbContext _context;

	public VendorBranchRepository(DbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<IEnumerable<ViewVendorBranch>> GetVendorBranchesByVendor(int vendorId)
	{
		return await _context.Set<ViewVendorBranch>()
			.Where(x => x.VendorId == vendorId)
			.AsNoTracking()
			.ToListAsync();
	}
}