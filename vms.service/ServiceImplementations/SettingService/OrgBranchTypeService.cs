using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class OrgBranchTypeService : ServiceBase<OrgBranchType>, IOrgBranchTypeService
{
    private readonly IOrgBranchTypeRepository _repository;

    public OrgBranchTypeService(IOrgBranchTypeRepository repository) : base(repository)
    {
        _repository = repository;
    }


    public async Task<IEnumerable<OrgBranchType>> GetOrgBranchTypeList()
    {
        return await _repository.GetOrgBranchType();
    }

    public async Task<IEnumerable<OrgBranchType>> GetOrgBranchTypeSelectList()
    {
        return await _repository.GetOrgBranchType();
    }
}