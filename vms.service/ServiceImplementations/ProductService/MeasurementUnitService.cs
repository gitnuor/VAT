using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.ProductService;

namespace vms.service.ServiceImplementations.ProductService;

public class MeasurementUnitService : ServiceBase<MeasurementUnit>, IMeasurementUnitService
{
    private readonly IMeasurementUnitRepository _repository;

    public MeasurementUnitService(IMeasurementUnitRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MeasurementUnit>> GetMeasurementUnits(string orgIdEnc)
    {
        return await _repository.GetMeasurementUnits(orgIdEnc);
    }

    public async Task<MeasurementUnit> GetMeasurementUnit(string idEnc)
    {
        return await _repository.GetMeasurementUnit(idEnc);
    }

	public async Task<MeasurementUnit> GetMeasurementUnitByName(int organizationId, string unitName)
	{
		return await _repository.GetMeasurementUnitByName(organizationId, unitName);
	}

	public async Task<IEnumerable<CustomSelectListItem>> GetMeasurementUnitSelectList(string pOrgId)
    {
        var units = await _repository.GetMeasurementUnits(pOrgId);
        return units.ConvertToCustomSelectList(nameof(MeasurementUnit.MeasurementUnitId),
            nameof(MeasurementUnit.Name));
    }
}