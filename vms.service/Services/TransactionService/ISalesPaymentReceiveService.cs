using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.TransactionService;

public interface ISalesPaymentReceiveService : IServiceBase<SalesPaymentReceive>
{
    Task<bool> ManageSalesDueAsync(VmSalesPaymentReceive vmSales);
}