using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class MeasurementUnitRepository : RepositoryBase<MeasurementUnit>, IMeasurementUnitRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public MeasurementUnitRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<MeasurementUnit>> GetMeasurementUnits(string orgIdEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var measurementUnits = await Query().Where(w => w.OrganizationId == id).SelectAsync();

        measurementUnits.ToList().ForEach(delegate (MeasurementUnit measurementUnit)
        {
            measurementUnit.EncryptedId = _dataProtector.Protect(measurementUnit.MeasurementUnitId.ToString());
        });
        return measurementUnits;
    }
    public async Task<MeasurementUnit> GetMeasurementUnit(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var measurementUnit = await Query()
            .SingleOrDefaultAsync(x => x.MeasurementUnitId == id, System.Threading.CancellationToken.None);
        measurementUnit.EncryptedId = _dataProtector.Protect(measurementUnit.MeasurementUnitId.ToString());

        return measurementUnit;
    }

	public async Task<MeasurementUnit> GetMeasurementUnitByName(int organizationId, string unitName)
	{
		var measurementUnit = await Query()
			.SingleOrDefaultAsync(x => x.OrganizationId == organizationId && x.Name == unitName, System.Threading.CancellationToken.None);
		return measurementUnit;
	}

}