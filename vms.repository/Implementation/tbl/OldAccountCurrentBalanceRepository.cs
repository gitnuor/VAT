using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class OldAccountCurrentBalanceRepository : RepositoryBase<OldAccountCurrentBalance>, IOldAccountCurrentBalanceRepository
{
    private readonly DbContext _context;

    public OldAccountCurrentBalanceRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}