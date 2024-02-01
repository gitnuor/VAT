using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class CountryService : ServiceBase<Country>, ICountryService
{
    private readonly ICountryRepository _repository;
    public CountryService(ICountryRepository repository) : base(repository)
    {
        _repository = repository;
    }

   

    public async Task<IEnumerable<Country>> GetCountryList()
    {
        return await _repository.Query().SelectAsync();

    }

    public async Task<IEnumerable<CustomSelectListItem>> CountrySelectList()
    {
        return (await _repository.Query().SelectAsync())
            .ConvertToCustomSelectList(nameof(Country.CountryId),
                nameof(Country.Name));
    }
}