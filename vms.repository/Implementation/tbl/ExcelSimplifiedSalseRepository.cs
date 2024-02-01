using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ExcelSimplifiedSalseRepository : RepositoryBase<ExcelSimplifiedSalse>, IExcelSimplifiedSalseRepository
{
    private readonly DbContext _context;

    public ExcelSimplifiedSalseRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}