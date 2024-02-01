using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IDistrictOrCityRepository : IRepositoryBase<DistrictOrCity>
{
    Task<IEnumerable<DistrictOrCity>> GetDistrictOrCities(int orgId);
    Task<IEnumerable<DistrictOrCity>> GetDistrictOrCitiesByDivisionOrState(int orgId, int divisionOrStateId);
    Task<IEnumerable<DistrictOrCity>> GetDistrictOrCitiesByCountry(int orgId, int countryId);
    Task<DistrictOrCity> GetDistrictOrCityById(string idEnc);
}