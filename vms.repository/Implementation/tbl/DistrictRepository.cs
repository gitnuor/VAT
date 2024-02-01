using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class DistrictRepository : RepositoryBase<District>, IDistrictRepository
{
    private readonly DbContext _context;

    public DistrictRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}