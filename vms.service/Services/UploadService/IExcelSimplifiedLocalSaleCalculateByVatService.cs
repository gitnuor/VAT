using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.UploadService;

public interface IExcelSimplifiedLocalSaleCalculateByVatService : IServiceBase<ExcelSimplifiedLocalSaleCalculateByVat>
{
	Task SaveSimplifiedLocalSaleCalculatedByVatList(List<VmExcelSimplifiedLocalSalesCalculateByVat> sales, long excelDataUploadId);
}