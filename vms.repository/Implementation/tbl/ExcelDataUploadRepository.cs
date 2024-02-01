using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ExcelDataUploadRepository : RepositoryBase<ExcelDataUpload>, IExcelDataUploadRepository
{
    private readonly DbContext _context;

    public ExcelDataUploadRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}