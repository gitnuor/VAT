using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class OrgStaticDataRepository : RepositoryBase<OrgStaticDatum>, IOrgStaticDataRepository
{
    private readonly DbContext _context;

    public OrgStaticDataRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}