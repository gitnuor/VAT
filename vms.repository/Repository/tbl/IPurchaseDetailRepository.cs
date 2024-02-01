using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IPurchaseDetailRepository : IRepositoryBase<PurchaseDetail>
{
    Task<IEnumerable<PurchaseDetail>> GetPurchaseDetails(string idEnc);
}