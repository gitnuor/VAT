using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class OrganizationService : ServiceBase<Organization>, IOrganizationService
{
    private readonly IOrganizationRepository _repository;

    public OrganizationService(IOrganizationRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Organization>> GetOrganizations(int orgIdEnc)
    {
        return await _repository.GetOrganizations(orgIdEnc);
    }

    public async Task<Organization> GetOrganization(string idEnc)
    {
        return await _repository.GetOrganization(idEnc);
    }

    public async Task<IEnumerable<Organization>> GetParentOrganizations(int par_orgId)
    {
        return await _repository.GetParentOrganizations(par_orgId);
    }
}