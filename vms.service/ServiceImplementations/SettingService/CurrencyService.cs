using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class CurrencyService : ServiceBase<Currency>, ICurrencyService
{
    private readonly ICurrencyRepository _repository;
    public CurrencyService(ICurrencyRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<SelectList> GetCurrencySelectList()
    {
        return new(await _repository.Query().SelectAsync(),
            nameof(Currency.CurrencyId),
            nameof(Currency.CurrencyName));
    }

    public async Task<IEnumerable<Currency>> GetCurrencyList()
    {
        return await _repository.Query().SelectAsync();

    }
}