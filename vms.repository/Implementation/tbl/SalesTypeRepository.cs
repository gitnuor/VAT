using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class SalesTypeRepository : RepositoryBase<SalesType>, ISalesTypeRepository
{
    private readonly DbContext _context;

    public SalesTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}