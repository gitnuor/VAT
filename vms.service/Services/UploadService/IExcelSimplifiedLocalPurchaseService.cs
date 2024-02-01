using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.UploadService;

public interface IExcelSimplifiedLocalPurchaseService : IServiceBase<ExcelSimplifiedLocalPurchase>
{
    Task SaveSimplifiedPurchaseList(List<VmExcelSimplifiedLocalPurchase> purchases, long excelDataUploadId);
}