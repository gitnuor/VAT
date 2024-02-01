using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class CustomerBranchRepository : RepositoryBase<CustomerBranch>, ICustomerBranchRepository
{
    private readonly DbContext _context;

    public CustomerBranchRepository(DbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ViewCustomerBranch>> GetCustomerBranchesByCustomer(int customerId)
    {
	    return await _context.Set<ViewCustomerBranch>()
		    .Where(x => x.CustomerId == customerId)
		    .AsNoTracking()
		    .ToListAsync();
    }
}