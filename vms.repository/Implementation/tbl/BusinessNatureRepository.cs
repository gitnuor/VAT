using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class BusinessNatureRepository : RepositoryBase<BusinessNature>, IBusinessNatureRepository
{
    private readonly DbContext _context;

    public BusinessNatureRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}