using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IContractVendorRepository : IRepositoryBase<ContractualProduction>
{
    Task<IEnumerable<ContractualProduction>> GetAll();

    Task<IEnumerable<ContractualProduction>> GetContractualProductions(int orgIdEnc);
    Task<ContractualProduction> GetTransferContract(string idEnc);
    Task<IEnumerable<ViewContractualProduction>> ViewContractualProduction(string orgIdEnc);
}