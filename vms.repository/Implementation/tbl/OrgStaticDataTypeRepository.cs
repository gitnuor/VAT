using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class OrgStaticDataTypeRepository : RepositoryBase<OrgStaticDataType>, IOrgStaticDataTypeRepository
{
    private readonly DbContext _context;

    public OrgStaticDataTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}