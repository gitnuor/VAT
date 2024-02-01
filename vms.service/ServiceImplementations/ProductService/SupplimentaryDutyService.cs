using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class SupplimentaryDutyService : ServiceBase<SupplimentaryDuty>, ISupplimentaryDutyService
{
    private readonly ISupplimentaryDutyRepository _repository;

    public SupplimentaryDutyService(ISupplimentaryDutyRepository repository) : base(repository)
    {
        _repository = repository;
    }
}