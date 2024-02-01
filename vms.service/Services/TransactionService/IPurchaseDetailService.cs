using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface IPurchaseDetailService : IServiceBase<PurchaseDetail>
{
    Task<IEnumerable<PurchaseDetail>> GetPurchaseDetails(string idEnc);
}