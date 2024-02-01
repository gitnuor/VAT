using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.TransactionService;

namespace vms.service.ServiceImplementations.TransactionService;

public class ContractVendorService : ServiceBase<ContractualProduction>, IContractVendorService
{
    private readonly IContractVendorRepository _repository;

    public ContractVendorService(IContractVendorRepository repository) : base(repository)
    {
        _repository = repository;
    }


    public async Task<IEnumerable<ContractualProduction>> GetContractualProductions(int orgIdEnc)
    {
        return await _repository.GetContractualProductions(orgIdEnc);
    }

    public async Task<ContractualProduction> GetTransferContract(string idEnc)
    {
        return await _repository.GetTransferContract(idEnc);
    }


    public async Task<IEnumerable<ContractualProduction>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<SelectList> GetContractualProductionsSelectList(int pOrgId)
    {
        return new(await _repository.GetContractualProductions(pOrgId),
            nameof(ContractualProduction.ContractualProductionId),
            nameof(ContractualProduction.ContractNo));
    }

    public async Task<IEnumerable<ViewContractualProduction>> ViewContractualProductions(string orgIdEnc)
    {
        return await _repository.ViewContractualProduction(orgIdEnc);
    }
}