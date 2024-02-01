using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;
using vms.entity.Utility;

namespace vms.service.ServiceImplementations.SettingService;

public class DamageTypeService : ServiceBase<DamageType>, IDamageTypeService
{
    private readonly IDamageTypeRepository _repository;

    public DamageTypeService(IDamageTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DamageType>> GetDamageTypes(int orgIdEnc)
    {
        return await _repository.GetDamageTypes(orgIdEnc);
    }

    public async Task<DamageType> GetDamageType(string idEnc)
    {
        return await _repository.GetDamageType(idEnc);
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetDamageTypeSelectList(int orgId)
    {
        var damageTypes = await _repository.GetDamageTypes(orgId);
        return damageTypes.ConvertToCustomSelectList(nameof(DamageType.DamageTypeId),
            nameof(DamageType.Name));
    }
}