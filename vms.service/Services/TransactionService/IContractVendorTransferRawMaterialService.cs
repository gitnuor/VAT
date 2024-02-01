using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.TransactionService;

public interface IContractVendorTransferRawMaterialService : IServiceBase<ContractualProductionTransferRawMaterial>
{
    Task<IEnumerable<ContractualProductionTransferRawMaterial>> GetTransferedRawMaterials(string idEnc);
}