using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IVehicleTypeRepository : IRepositoryBase<VehicleType>
{
    Task<IEnumerable<VehicleType>> GetVehicleTypes(int orgId);
    Task<VehicleType> GetVehicleType(string idEnc);
}