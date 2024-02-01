using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;
using vms.entity.Utility;

namespace vms.service.ServiceImplementations.SettingService;

public class DivisionOrStateService : ServiceBase<DivisionOrState>, IDivisionOrStateService
{
    private readonly IDivisionOrStateRepository _repository;

    public DivisionOrStateService(IDivisionOrStateRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<SelectList> GetDivisionOrStateSelectList(int orgId)
    {
        return new(await _repository.GetDivisionOrStates(orgId),
            nameof(DivisionOrState.DivisionOrStateId),
            nameof(DivisionOrState.DivisionOrStateName));
    }

    public async Task<IEnumerable<DivisionOrState>> GetDivisionOrStates(int orgId)
    {
        return await _repository.GetDivisionOrStates(orgId);
    }

    public async Task<DivisionOrState> GetDivisionOrState(string encryptedId)
    {
        return await _repository.GetDivisionOrStateById(encryptedId);
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetDivisionSelectListItem(int orgId)
    {
        var list = await _repository.Query().Where(x => x.OrganizationId == orgId).SelectAsync();
        return list.ConvertToCustomSelectList(nameof(DivisionOrState.DivisionOrStateId), nameof(DivisionOrState.DivisionOrStateName));
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetDivisionsByCountryId(int orgId, int countryId)
    {
        var list = await _repository.GetDivisionOrStatesByCountryId(orgId, countryId);
        //return list.Select(x => new SelectListItem
        //{ Value = x.DivisionOrStateId.ToString(), Text = x.DivisionOrStateName });
        return list.ConvertToCustomSelectList(nameof(DivisionOrState.DivisionOrStateId), nameof(DivisionOrState.DivisionOrStateName));
    }
}