using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.UploadService;

public interface IExcelSimplifiedPurchaseService : IServiceBase<ExcelSimplifiedPurchase>
{
    Task SaveSimplifiedPurchaseList(List<VmExcelSimplifiedPurchase> purchases, long excelDataUploadId);
}