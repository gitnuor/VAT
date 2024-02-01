using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class DistrictService : ServiceBase<District>, IDistrictService
{
    private readonly IDistrictRepository _repository;
    public DistrictService(IDistrictRepository repository) : base(repository)
    {
        _repository = repository;
    }


    public async Task<IEnumerable<District>> GetDistrictList()
    {
        return await _repository.Query().SelectAsync();
    }

    public async Task<IEnumerable<CustomSelectListItem>> DistrictSelectList()
    {
        return (await _repository.Query().SelectAsync())
            .ConvertToCustomSelectList(nameof(District.DistrictId),
            nameof(District.Name));
    }
}