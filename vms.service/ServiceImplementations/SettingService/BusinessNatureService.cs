using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class BusinessNatureService : ServiceBase<BusinessNature>, IBusinessNatureService
{
    private readonly IBusinessNatureRepository _repository;
    public BusinessNatureService(IBusinessNatureRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SelectListItem>> GetAllNatureSelectListItem()
    {
        var list = await _repository.Query().SelectAsync();
        return list.Select(x => new SelectListItem { Value = x.BusinessNatureId.ToString(), Text = x.Name });
    }

    public async Task<IEnumerable<CustomSelectListItem>> BusinessNatureSelectList()
    {
        return (await _repository.Query().SelectAsync())
            .ConvertToCustomSelectList(nameof(BusinessNature.BusinessNatureId),
                nameof(BusinessNature.Name));
    }
}