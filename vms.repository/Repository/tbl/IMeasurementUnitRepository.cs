using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IMeasurementUnitRepository : IRepositoryBase<MeasurementUnit>
{
    Task<IEnumerable<MeasurementUnit>> GetMeasurementUnits(string orgIdEnc);
    Task<MeasurementUnit> GetMeasurementUnit(string idEnc);
    Task<MeasurementUnit> GetMeasurementUnitByName(int organizationId, string unitName);
}