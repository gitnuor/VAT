using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;
using vms.entity.Utility;
using vms.entity.viewModels;

namespace vms.service.ServiceImplementations.SettingService;

public class DistrictOrCityService : ServiceBase<DistrictOrCity>, IDistrictOrCityService
{
    private readonly IDistrictOrCityRepository _repository;

    public DistrictOrCityService(IDistrictOrCityRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetDistrictOrCitySelectList(int orgId)
    {
        var districtOrCities = await _repository.GetDistrictOrCities(orgId);
        return districtOrCities.ConvertToCustomSelectList(nameof(DistrictOrCity.DistrictOrCityId),
            nameof(DistrictOrCity.DistrictOrCityName));
    }

    public async Task<IEnumerable<DistrictOrCity>> GetDistrictOrCities(int orgId)
    {
        return await _repository.GetDistrictOrCities(orgId);
    }

    public async Task<DistrictOrCity> GetDistrictOrCity(string encryptedId)
    {
        return await _repository.GetDistrictOrCityById(encryptedId);
    }

    public async Task<IEnumerable<SelectListItem>> GetDistrictSelectListItem(int orgId)
    {
        var list = await _repository.Query().Where(x => x.OrganizationId == orgId).SelectAsync();
        return list.Select(x => new SelectListItem
        { Value = x.DistrictOrCityId.ToString(), Text = x.DistrictOrCityName });
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetDistrictsByDivisionId(int orgId, int divisionId)
    {
        var list = await _repository.GetDistrictOrCitiesByDivisionOrState(orgId, divisionId);
        //return list.Select(x => new SelectListItem
        //{ Value = x.DistrictOrCityId.ToString(), Text = x.DistrictOrCityName });

        return list.ConvertToCustomSelectList(nameof(DistrictOrCity.DistrictOrCityId),
           nameof(DistrictOrCity.DistrictOrCityName));
    }

    public async Task<IEnumerable<SelectListItem>> GetDistrictsByCountryId(int orgId, int countryId)
    {
        var list = await _repository.GetDistrictOrCitiesByCountry(orgId, countryId);
        return list.Select(x => new SelectListItem
        { Value = x.DistrictOrCityId.ToString(), Text = x.DistrictOrCityName });
    }
}