using System;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.PaymentService;

namespace vms.service.ServiceImplementations.PaymentService;

public class OldAccountCurrentBalanceService : ServiceBase<OldAccountCurrentBalance>, IOldAccountCurrentBalanceService
{
    private readonly IOldAccountCurrentBalanceRepository _repository;

    public OldAccountCurrentBalanceService(IOldAccountCurrentBalanceRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsMonthlyCurrentBalanceExist(OldAccountCurrentBalance oldAccountCurrentBalance)
    {
        return await _repository.Query().AnyAsync(x =>
            x.OrganizationId == oldAccountCurrentBalance.OrganizationId &&
            x.MushakYear == oldAccountCurrentBalance.MushakYear &&
            x.MushakMonth == oldAccountCurrentBalance.MushakMonth);
    }
}