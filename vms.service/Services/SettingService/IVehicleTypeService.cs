using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IVehicleTypeService : IServiceBase<VehicleType>
{
    Task<IEnumerable<CustomSelectListItem>> GetVehicleTypeSelectList(int orgId);
    Task<IEnumerable<VehicleType>> GetVehicleTypes(int orgId);
    Task<VehicleType> GetVehicleType(string encryptedId);
}