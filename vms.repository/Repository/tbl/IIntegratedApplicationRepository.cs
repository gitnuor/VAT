using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.repository.Repository.tbl;

public interface IIntegratedApplicationRepository : IRepositoryBase<IntegratedApplication>
{
    Task<IEnumerable<IntegratedApplication>> GetIntegratedApplications(int orgId);
    Task<IntegratedApplication> GetIntegratedApplication(string idEnc);
    Task<IntegratedApplication> GetIntegratedApplicationByAppKey(string appKey);
}