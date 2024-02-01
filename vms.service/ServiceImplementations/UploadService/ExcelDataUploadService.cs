using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class ExcelDataUploadService : ServiceBase<ExcelDataUpload>, IExcelDataUploadService
{
    private readonly IExcelDataUploadRepository _repository;
    public ExcelDataUploadService(IExcelDataUploadRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ExcelDataUpload>> GetSimplifiedLocalPurchaseListAsync()
    {
        return await _repository.Query()
            .Where(edu => edu.ExcelUploadedDataTypeId == (int)EnumExcelDataUploadType.SimplifiedLocalPurchase)
            .Include(edu => edu.ExcelUploadedDataType)
            .Include(edu => edu.ExcelSimplifiedLocalPurchases)
            .OrderByDescending(edu => edu.ExcelDataUploadId)
            .SelectAsync();
    }

    public async Task<ExcelDataUpload> GetSimplifiedLocalPurchaseAsync(int id)
    {
        return await _repository.Query()
            .Include(edu => edu.ExcelUploadedDataType)
            .Include(edu => edu.ExcelSimplifiedLocalPurchases)
            .FirstOrDefaultAsync(edu =>
                edu.ExcelDataUploadId == id && edu.ExcelUploadedDataTypeId ==
                (int)EnumExcelDataUploadType.SimplifiedLocalPurchase);
    }

    public async Task<IEnumerable<ExcelDataUpload>> GetSimplifiedLocalSalesListAsync()
    {
        return await _repository.Query()
            .Where(edu => edu.ExcelUploadedDataTypeId == (int)EnumExcelDataUploadType.SimplifiedLocalSale)
            .Include(edu => edu.ExcelUploadedDataType)
            .Include(edu => edu.ExcelSimplifiedSalses)
            .OrderByDescending(edu => edu.ExcelDataUploadId)
            .SelectAsync();
    }

    public async Task<ExcelDataUpload> GetSimplifiedLocalSaleAsync(int id)
    {
        return await _repository.Query()
            .Include(edu => edu.ExcelUploadedDataType)
            .Include(edu => edu.ExcelSimplifiedSalses)
            .FirstOrDefaultAsync(edu =>
                edu.ExcelDataUploadId == id && edu.ExcelUploadedDataTypeId ==
                (int)EnumExcelDataUploadType.SimplifiedLocalSale);
    }
}