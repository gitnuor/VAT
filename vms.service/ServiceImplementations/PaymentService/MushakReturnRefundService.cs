using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.PaymentService;

namespace vms.service.ServiceImplementations.PaymentService;

public class MushakReturnRefundService : ServiceBase<MushakReturnRefund>, IMushakReturnRefundService
{
    public MushakReturnRefundService(IMushakReturnRefundRepository repository) : base(repository)
    {
    }
}