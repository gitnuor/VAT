using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class TransectionTypeService : ServiceBase<TransectionType>, ITransectionTypeService
{
    private readonly ITransectionTypeRepository _repository;

    public TransectionTypeService(ITransectionTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }
}