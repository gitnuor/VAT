using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl.acc;

namespace vms.repository.Implementation.tbl.acc;

public class CoagroupRepository : RepositoryBase<Coagroup>, ICoagroupRepository
{
    private readonly DbContext _context;

    public CoagroupRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}