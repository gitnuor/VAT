using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.UploadService;

public interface IExcelSimplifiedSalseService : IServiceBase<ExcelSimplifiedSalse>
{
    Task SaveSimplifiedSaleList(List<VmExcelSimplifiedSales> sales, long excelDataUploadId);
    Task SaveSimplifiedLocalSaleWithoutPaymentList(List<VmExcelSimplifiedLocalSalesWithoutPayment> sales, long excelDataUploadId);
}