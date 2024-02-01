using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class ContractVendorTransferRawMaterialService : ServiceBase<ContractualProductionTransferRawMaterial>, IContractVendorTransferRawMaterialService
{
    private readonly IContractVendorTransferRawMaterialRepository _repository;
    public ContractVendorTransferRawMaterialService(IContractVendorTransferRawMaterialRepository repository) : base(repository)
    {
        _repository = repository;
    }



    public async Task<IEnumerable<ContractualProductionTransferRawMaterial>> GetTransferedRawMaterials(string idEnc)
    {
        return await _repository.GetTransferedRawMaterials(idEnc);
    }


}