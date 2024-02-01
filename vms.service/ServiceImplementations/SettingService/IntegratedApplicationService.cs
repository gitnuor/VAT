using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class IntegratedApplicationService : ServiceBase<IntegratedApplication>, IIntegratedApplicationService
{
    private readonly IIntegratedApplicationRepository _repository;

    public IntegratedApplicationService(IIntegratedApplicationRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<IntegratedApplication>> GetIntegratedApplications(int orgId)
    {
        return _repository.GetIntegratedApplications(orgId);
    }

    public Task<IntegratedApplication> GetIntegratedApplication(string idEnc)
    {
        return _repository.GetIntegratedApplication(idEnc);
    }

    public Task<IntegratedApplication> GetIntegratedApplicationByAppKey(string appKey)
    {
        return _repository.GetIntegratedApplicationByAppKey(appKey);
    }

    public Task<bool> IsIntegratedApplicationExists(string appKey)
    {
	    return _repository.Query().AnyAsync(x => x.ApplicationKey == appKey);
    }
}