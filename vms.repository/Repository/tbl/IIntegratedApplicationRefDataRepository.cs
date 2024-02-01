using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IIntegratedApplicationRefDataRepository : IRepositoryBase<IntegratedApplicationRefDatum>
{
    //Task<IEnumerable<IntegratedApplication>> GetIntegratedApplications(int orgId);
    Task<IntegratedApplicationRefDatum> GetIntegratedApplicationRefDatum(string idEnc);
    //Task<IntegratedApplication> GetIntegratedApplicationByAppKey(string appKey);
}