using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class PurchasePaymentService : ServiceBase<PurchasePayment>, IPurchasePaymentService
{
    private readonly IPurchasePaymentRepository _repository;
    public PurchasePaymentService(IPurchasePaymentRepository repository) : base(repository)
    {
        _repository = repository;
    }
}