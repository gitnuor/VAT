using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.repository.Repository.tbl;

public interface ISalesPaymentReceiveRepository : IRepositoryBase<SalesPaymentReceive>
{
    Task<bool> ManageSalesDueAsync(VmSalesPaymentReceive vmSales);
}