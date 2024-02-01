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

public class BusinessCategoryService : ServiceBase<BusinessCategory>, IBusinessCategoryService
{
    private readonly IBusinessCategoryRepository _repository;

    public BusinessCategoryService(IBusinessCategoryRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetAllCategorySelectListItem()
    {
        //var list = await _repository.Query().SelectAsync();
        //return list.Select(x => new SelectListItem { Value = x.BusinessCategoryId.ToString(), Text = x.Name });
        var list = await _repository.Query().SelectAsync();
        return list.ConvertToCustomSelectList(nameof(BusinessCategory.BusinessCategoryId),
            nameof(BusinessCategory.Name));
    }

    public async Task<IEnumerable<CustomSelectListItem>> BusinessCategorySelectList()
    {
        return (await _repository.Query().SelectAsync())
            .ConvertToCustomSelectList(nameof(BusinessCategory.BusinessCategoryId),
                nameof(BusinessCategory.Name));
    }
}