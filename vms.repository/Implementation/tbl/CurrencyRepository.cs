using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
{
    private readonly DbContext _context;

    public CurrencyRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}