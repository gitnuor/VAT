using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class ContractTypeService : ServiceBase<ContractType>, IContractTypeService
{
    public ContractTypeService(IContractTypeRepository repository) : base(repository)
    {
    }
}