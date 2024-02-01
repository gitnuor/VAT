using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class SalesPaymentReceiveService : ServiceBase<SalesPaymentReceive>, ISalesPaymentReceiveService
{
    private readonly ISalesPaymentReceiveRepository _repository;
    public SalesPaymentReceiveService(ISalesPaymentReceiveRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<bool> ManageSalesDueAsync(VmSalesPaymentReceive vmSales)
    {
        return await _repository.ManageSalesDueAsync(vmSales);
    }
}