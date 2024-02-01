using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class SalesPriceAdjustmentDetailService : ServiceBase<SalesPriceAdjustmentDetail>, ISalesPriceAdjustmentDetailService
{
    private readonly ISalesPriceAdjustmentDetailRepository _repository;
    public SalesPriceAdjustmentDetailService(ISalesPriceAdjustmentDetailRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SalesPriceAdjustmentDetail>> GetSalesPriceAdjustmentDetailsBySalesPriceAdjustment(string salesPriceAdjustmentIdEnc)
    {
        return await _repository.GetSalesPriceAdjustmentDetailsBySalesPriceAdjustment(salesPriceAdjustmentIdEnc);
    }
}