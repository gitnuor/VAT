using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class ContractVendorProductDetailsService : ServiceBase<ContractualProductionProductDetail>,
    IContractVendorProductDetailsService
{
    public ContractVendorProductDetailsService(IContractVendorProductDetailsRepository repository) : base(repository)
    {
    }
}