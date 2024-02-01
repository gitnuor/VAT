using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class ExcelSimplifiedLocalSaleCalculateByVatService(IExcelSimplifiedLocalSaleCalculateByVatRepository repository,
		IMapper mapper)
	: ServiceBase<ExcelSimplifiedLocalSaleCalculateByVat>(repository), IExcelSimplifiedLocalSaleCalculateByVatService
{
	public Task SaveSimplifiedLocalSaleCalculatedByVatList(List<VmExcelSimplifiedLocalSalesCalculateByVat> sales, long excelDataUploadId)
	{
		var salesList = mapper.Map<List<VmExcelSimplifiedLocalSalesCalculateByVat>, List<ExcelSimplifiedLocalSaleCalculateByVat>>(sales);

		foreach (var sale in salesList)
		{
			sale.ExcelDataUploadId = excelDataUploadId;
			sale.IsProcessed = false;
		}

		return repository.BulkInsertAsync(salesList);
	}
}