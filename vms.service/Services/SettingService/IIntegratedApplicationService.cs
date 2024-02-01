using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.SettingService;

public interface IIntegratedApplicationService : IServiceBase<IntegratedApplication>
{
    Task<IEnumerable<IntegratedApplication>> GetIntegratedApplications(int orgId);
    Task<IntegratedApplication> GetIntegratedApplication(string idEnc);
    Task<IntegratedApplication> GetIntegratedApplicationByAppKey(string appKey);
    Task<bool> IsIntegratedApplicationExists(string appKey);
}