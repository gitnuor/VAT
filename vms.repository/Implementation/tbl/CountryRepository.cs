using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class CountryRepository : RepositoryBase<Country>, ICountryRepository
{
    private readonly DbContext _context;

    public CountryRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}