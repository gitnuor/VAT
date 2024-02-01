using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.UploadService;

public interface IExcelDataUploadService : IServiceBase<ExcelDataUpload>
{
    Task<IEnumerable<ExcelDataUpload>> GetSimplifiedLocalPurchaseListAsync();
    Task<ExcelDataUpload> GetSimplifiedLocalPurchaseAsync(int id);
    Task<IEnumerable<ExcelDataUpload>> GetSimplifiedLocalSalesListAsync();
    Task<ExcelDataUpload> GetSimplifiedLocalSaleAsync(int id);
}