using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class BankBranchService : ServiceBase<BankBranch>, IBankBranchService
{
    public BankBranchService(IBankBranchRepository repository) : base(repository)
    {
    }
}