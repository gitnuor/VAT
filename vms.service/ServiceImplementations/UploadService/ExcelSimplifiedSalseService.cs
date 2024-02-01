using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class ExcelSimplifiedSalseService : ServiceBase<ExcelSimplifiedSalse>, IExcelSimplifiedSalseService
{
    private readonly IExcelSimplifiedSalseRepository _repository;
    private readonly IMapper _mapper;
    public ExcelSimplifiedSalseService(IExcelSimplifiedSalseRepository repository, IMapper mapper) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task SaveSimplifiedSaleList(List<VmExcelSimplifiedSales> sales, long excelDataUploadId)
    {
        var salesList = _mapper.Map<List<VmExcelSimplifiedSales>, List<ExcelSimplifiedSalse>>(sales);
        foreach (var sale in salesList)
        {
            sale.ExcelDataUploadId = excelDataUploadId;
            sale.IsProcessed = false;
        }

        return _repository.BulkInsertAsync(salesList);
    }

    public Task SaveSimplifiedLocalSaleWithoutPaymentList(List<VmExcelSimplifiedLocalSalesWithoutPayment> sales, long excelDataUploadId)
    {
		var salesList = _mapper.Map<List<VmExcelSimplifiedLocalSalesWithoutPayment>, List<ExcelSimplifiedSalse>>(sales);

		foreach (var sale in salesList)
		{
			sale.ExcelDataUploadId = excelDataUploadId;
            sale.IsVds = false;
            sale.VdsAmount = 0;
            sale.IsTds = false;
            sale.TdsAmount = 0;
			sale.IsProcessed = false;
		}

		return _repository.BulkInsertAsync(salesList);
	}
}