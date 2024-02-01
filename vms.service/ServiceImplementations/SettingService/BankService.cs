using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class BankService : ServiceBase<Bank>, IBankService
{
    private readonly IBankRepository _repository;

    public BankService(IBankRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<SelectList> GetBankSelectList()
    {
        return new(await _repository.GetBank(),
            nameof(Bank.BankId),
            nameof(Bank.Name));
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetBankSelectListItemByOrg(int orgId)
    {
        //var list = await _repository.Query()
        //	.Where(x => x.OrganizationId == orgId)
        //	.SelectAsync();
        //return list.Select(x =>
        //	new SelectListItem { Value = x.BankId.ToString(), Text = x.Name }

        var list = await _repository.Query()
        .Where(x => x.OrganizationId == orgId)
        .SelectAsync();
        return list.ConvertToCustomSelectList(nameof(Bank.BankId),
            nameof(Bank.Name));
    }
}