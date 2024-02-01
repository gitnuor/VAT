using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ObjectTypeRepository : RepositoryBase<ObjectType>, IObjectTypeRepository
{
    private readonly DbContext _context;

    public ObjectTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}