using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class ContractVendorTransferRawMaterialDetailsService : ServiceBase<ContractualProductionTransferRawMaterialDetail>, IContractVendorTransferRawMaterialDetailsService
{
    private readonly IContractVendorTransferRawMaterialDetailsRepository _repository;
    public ContractVendorTransferRawMaterialDetailsService(IContractVendorTransferRawMaterialDetailsRepository repository) : base(repository)
    {
        _repository = repository;
    }
}