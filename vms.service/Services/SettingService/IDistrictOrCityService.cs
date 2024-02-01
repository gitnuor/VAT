using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IDistrictOrCityService : IServiceBase<DistrictOrCity>
{
    Task<IEnumerable<CustomSelectListItem>> GetDistrictOrCitySelectList(int orgId);
    Task<IEnumerable<DistrictOrCity>> GetDistrictOrCities(int orgId);
    Task<DistrictOrCity> GetDistrictOrCity(string encryptedId);
    Task<IEnumerable<SelectListItem>> GetDistrictSelectListItem(int orgId);
    Task<IEnumerable<CustomSelectListItem>> GetDistrictsByDivisionId(int orgId, int divisionId);
    Task<IEnumerable<SelectListItem>> GetDistrictsByCountryId(int orgId, int countryId);
}