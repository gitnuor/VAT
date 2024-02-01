using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface ISalesPriceAdjustmentDetailRepository : IRepositoryBase<SalesPriceAdjustmentDetail>
{
    Task<IEnumerable<SalesPriceAdjustmentDetail>> GetSalesPriceAdjustmentDetailsBySalesPriceAdjustment(string salesPriceAdjustmentIdEnc);
}