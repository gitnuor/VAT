using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.UploadService;

namespace vms.service.ServiceImplementations.UploadService;

public class ExcelSimplifiedPurchaseService : ServiceBase<ExcelSimplifiedPurchase>, IExcelSimplifiedPurchaseService
{
    private readonly IExcelSimplifiedPurchaseRepository _repository;
    private readonly IMapper _mapper;
    public ExcelSimplifiedPurchaseService(IExcelSimplifiedPurchaseRepository repository, IMapper mapper) : base(repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task SaveSimplifiedPurchaseList(List<VmExcelSimplifiedPurchase> purchases, long excelDataUploadId)
    {
        var purchaseList = _mapper.Map<List<VmExcelSimplifiedPurchase>, List<ExcelSimplifiedPurchase>>(purchases);
        foreach (var purchase in purchaseList)
        {
            purchase.ExcelDataUploadId = excelDataUploadId;
            purchase.IsProcessed = false;
        }

        return _repository.BulkInsertAsync(purchaseList);
    }
}