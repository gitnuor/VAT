using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class TransectionTypeRepository : RepositoryBase<TransectionType>, ITransectionTypeRepository
{
    private readonly DbContext _context;

    public TransectionTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}