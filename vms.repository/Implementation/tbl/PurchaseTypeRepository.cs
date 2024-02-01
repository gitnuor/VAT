using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PurchaseTypeRepository : RepositoryBase<PurchaseType>, IPurchaseTypeRepository
{
    private readonly DbContext _context;

    public PurchaseTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}