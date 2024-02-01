using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;
using vms.entity.Utility;

namespace vms.service.ServiceImplementations.SettingService;

public class AdjustmentTypeService : ServiceBase<AdjustmentType>, IAdjustmentTypeService
{
    private readonly IAdjustmentTypeRepository _repository;
    public AdjustmentTypeService(IAdjustmentTypeRepository repository) : base(repository)
    {
        _repository= repository;
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetAdjustmentTypeSelectList()
    {
        var adjustmentTypes = await _repository.Query().SelectAsync();
        return adjustmentTypes.ConvertToCustomSelectList(nameof(AdjustmentType.AdjustmentTypeId),
            nameof(AdjustmentType.Name));
    }
}