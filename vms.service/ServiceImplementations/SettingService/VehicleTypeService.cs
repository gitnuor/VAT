using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class VehicleTypeService : ServiceBase<VehicleType>, IVehicleTypeService
{
    private readonly IVehicleTypeRepository _repository;

    public VehicleTypeService(IVehicleTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetVehicleTypeSelectList(int orgId)
    {
        var vehicleList = await _repository.GetVehicleTypes(orgId);
        return vehicleList.ConvertToCustomSelectList(nameof(VehicleType.VehicleTypeId),
            nameof(VehicleType.VehicleTypeName));
    }

    public async Task<IEnumerable<VehicleType>> GetVehicleTypes(int orgId)
    {
        return await _repository.GetVehicleTypes(orgId);
    }

    public async Task<VehicleType> GetVehicleType(string encryptedId)
    {
        return await _repository.GetVehicleType(encryptedId);
    }
}