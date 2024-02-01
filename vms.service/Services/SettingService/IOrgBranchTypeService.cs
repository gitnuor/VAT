using System.Collections.Generic;
using System.Threading.Tasks;
using vms.entity.models;

namespace vms.service.Services.SettingService;

public interface IOrgBranchTypeService : IServiceBase<OrgBranchType>
{
    Task<IEnumerable<OrgBranchType>> GetOrgBranchTypeSelectList();
    Task<IEnumerable<OrgBranchType>> GetOrgBranchTypeList();
}