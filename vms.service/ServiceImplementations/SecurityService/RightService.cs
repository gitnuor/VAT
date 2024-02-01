using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SecurityService;

namespace vms.service.ServiceImplementations.SecurityService;

public class RightService : ServiceBase<Right>, IRightService
{
    private readonly IRightRepository _repository;

    public RightService(IRightRepository repository) : base(repository)
    {
        _repository = repository;
    }
}