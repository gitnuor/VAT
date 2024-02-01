using System;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.PaymentService;

public interface IOldAccountCurrentBalanceService : IServiceBase<OldAccountCurrentBalance>
{
    Task<bool> IsMonthlyCurrentBalanceExist(OldAccountCurrentBalance oldAccountCurrentBalance);
}