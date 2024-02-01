using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class ExcelUploadedDataTypeRepository : RepositoryBase<ExcelUploadedDataType>, IExcelUploadedDataTypeRepository
{
    private readonly DbContext _context;

    public ExcelUploadedDataTypeRepository(DbContext context) : base(context)
    {
        _context = context;
    }
}