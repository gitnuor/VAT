using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.ProductService;

public interface IMeasurementUnitService : IServiceBase<MeasurementUnit>
{
    Task<IEnumerable<MeasurementUnit>> GetMeasurementUnits(string orgIdEnc);
    Task<MeasurementUnit> GetMeasurementUnit(string idEnc);
    Task<IEnumerable<CustomSelectListItem>> GetMeasurementUnitSelectList(string pOrgId);
    Task<MeasurementUnit> GetMeasurementUnitByName(int organizationId, string unitName);
}