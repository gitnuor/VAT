using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IDivisionOrStateService : IServiceBase<DivisionOrState>
{
    Task<SelectList> GetDivisionOrStateSelectList(int orgId);
    Task<IEnumerable<DivisionOrState>> GetDivisionOrStates(int orgId);
    Task<DivisionOrState> GetDivisionOrState(string encryptedId);
    Task<IEnumerable<CustomSelectListItem>> GetDivisionSelectListItem(int orgId);
    Task<IEnumerable<CustomSelectListItem>> GetDivisionsByCountryId(int orgId, int countryId);
}