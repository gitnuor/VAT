using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ExcelSimplifiedLocalPurchaseRepository : RepositoryBase<ExcelSimplifiedLocalPurchase>, IExcelSimplifiedLocalPurchaseRepository
{
    private readonly DbContext _context;

    public ExcelSimplifiedLocalPurchaseRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}