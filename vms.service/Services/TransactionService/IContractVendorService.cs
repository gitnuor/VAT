using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.TransactionService;

public interface IContractVendorService : IServiceBase<ContractualProduction>
{
    Task<IEnumerable<ContractualProduction>> GetAll();

    Task<IEnumerable<ContractualProduction>> GetContractualProductions(int orgIdEnc);
    Task<ContractualProduction> GetTransferContract(string idEnc);

    Task<SelectList> GetContractualProductionsSelectList(int pOrgId);
    Task<IEnumerable<ViewContractualProduction>> ViewContractualProductions(string orgIdEnc);
}