using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class RightRepository : RepositoryBase<Right>, IRightRepository
{
    private readonly DbContext _context;

    public RightRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}