using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class BankBranchRepository : RepositoryBase<BankBranch>, IBankBranchRepository
{
    private readonly DbContext _context;

    public BankBranchRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}