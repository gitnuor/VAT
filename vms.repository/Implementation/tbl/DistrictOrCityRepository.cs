using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class DistrictOrCityRepository : RepositoryBase<DistrictOrCity>, IDistrictOrCityRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public DistrictOrCityRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<DistrictOrCity>> GetDistrictOrCities(int orgId)
    {
        var districtOrCities = await Query().Include(x=>x.DivisionOrState).Where(w => w.OrganizationId == orgId).SelectAsync();

        districtOrCities.ToList().ForEach(delegate (DistrictOrCity type)
        {
            type.EncryptedId = _dataProtector.Protect(type.DivisionOrStateId.ToString());
        });
        return districtOrCities.ToList();
    }

    public async Task<IEnumerable<DistrictOrCity>> GetDistrictOrCitiesByCountry(int orgId, int countryId)
    {
        var districtOrCities = await Query().Where(w => w.OrganizationId == orgId && w.DivisionOrState.CountryId == countryId).SelectAsync();
                       
        return districtOrCities.ToList();
    }

    public async Task<IEnumerable<DistrictOrCity>> GetDistrictOrCitiesByDivisionOrState(int orgId, int divisionOrStateId)
    {
        var districtOrCities = await Query().Where(w => w.OrganizationId == orgId && w.DivisionOrStateId == divisionOrStateId).SelectAsync();
                        
        return districtOrCities.ToList();
    }

    public async Task<DistrictOrCity> GetDistrictOrCityById(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var districtOrCity = await Query()
            .SingleOrDefaultAsync(x => x.DistrictOrCityId == id, System.Threading.CancellationToken.None);
        districtOrCity.EncryptedId = _dataProtector.Protect(districtOrCity.DistrictOrCityId.ToString());

        return districtOrCity;
    }
}