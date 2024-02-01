using Microsoft.EntityFrameworkCore;
using vms.repository.Repository.sp;

namespace vms.repository.Implementation.sp;

public class DataImportRepository : IDataImportRepository
{
    private readonly DbContext _context;
    public DataImportRepository(DbContext context)
    {
        _context = context;
    }
        
}