using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class ExcelSimplifiedLocalPurchaseService : ServiceBase<ExcelSimplifiedLocalPurchase>, IExcelSimplifiedLocalPurchaseService
{
    private readonly IExcelSimplifiedLocalPurchaseRepository _repository;
    private readonly IMapper _mapper;
    public ExcelSimplifiedLocalPurchaseService(IExcelSimplifiedLocalPurchaseRepository repository, IMapper mapper) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task SaveSimplifiedPurchaseList(List<VmExcelSimplifiedLocalPurchase> purchases, long excelDataUploadId)
    {
        var purchaseList = _mapper.Map<List<VmExcelSimplifiedLocalPurchase>, List<ExcelSimplifiedLocalPurchase>>(purchases);
        foreach (var purchase in purchaseList)
        {
            purchase.ExcelDataUploadId = excelDataUploadId;
            purchase.IsProcessed = false;
        }

        return _repository.BulkInsertAsync(purchaseList);
    }
}