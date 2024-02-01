using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ExcelSimplifiedPurchaseRepository : RepositoryBase<ExcelSimplifiedPurchase>, IExcelSimplifiedPurchaseRepository
{
    private readonly DbContext _context;

    public ExcelSimplifiedPurchaseRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}