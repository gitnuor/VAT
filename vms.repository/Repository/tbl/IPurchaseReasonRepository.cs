using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IPurchaseReasonRepository : IRepositoryBase<PurchaseReason>
{
    Task<IEnumerable<PurchaseReason>> GetPurchaseReasons();
    Task<PurchaseReason> GetPurchaseReason(string idEnc);
}