using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IContractVendorTransferRawMaterialRepository : IRepositoryBase<ContractualProductionTransferRawMaterial>
{
    Task<IEnumerable<ContractualProductionTransferRawMaterial>> GetTransferedRawMaterials(string idEnc);
}