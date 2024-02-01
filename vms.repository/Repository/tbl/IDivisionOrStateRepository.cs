using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IDivisionOrStateRepository : IRepositoryBase<DivisionOrState>
{
    Task<IEnumerable<DivisionOrState>> GetDivisionOrStates(int orgId);
    Task<IEnumerable<DivisionOrState>> GetDivisionOrStatesByCountryId(int orgId,int countryId);
    Task<DivisionOrState> GetDivisionOrStateById(string idEnc);
}