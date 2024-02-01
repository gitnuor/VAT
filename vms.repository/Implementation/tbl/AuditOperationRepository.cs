using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class AuditOperationRepository : RepositoryBase<AuditOperation>, IAuditOperationRepository
{
    private readonly DbContext _context;

    public AuditOperationRepository(DbContext context) : base(context)
    {
        _context = context;
    }

}