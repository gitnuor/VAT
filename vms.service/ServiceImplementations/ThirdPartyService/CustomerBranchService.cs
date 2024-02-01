using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.ThirdPartyService;

namespace vms.service.ServiceImplementations.ThirdPartyService;

public class CustomerBranchService : ServiceBase<CustomerBranch>, ICustomerBranchService
{
    public CustomerBranchService(ICustomerBranchRepository repository) : base(repository)
    {
    }
}