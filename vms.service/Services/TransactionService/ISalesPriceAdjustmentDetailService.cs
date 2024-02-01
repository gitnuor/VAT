using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface ISalesPriceAdjustmentDetailService : IServiceBase<SalesPriceAdjustmentDetail>
{
    Task<IEnumerable<SalesPriceAdjustmentDetail>> GetSalesPriceAdjustmentDetailsBySalesPriceAdjustment(string salesPriceAdjustmentIdEnc);
}