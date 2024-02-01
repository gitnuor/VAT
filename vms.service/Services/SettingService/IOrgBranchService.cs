using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.viewModels;

namespace vms.service.Services.SettingService;

public interface IOrgBranchService : IServiceBase<OrgBranch>
{
    Task<IEnumerable<OrgBranch>> GetOrgBranchList();
    Task<IEnumerable<OrgBranch>> GetOrgBranchByOrganization(string pOrgId);
    Task<OrgBranch> GetOrgBranch(string idEnc);
    Task<IEnumerable<CustomSelectListItem>> GetOrgBranchSelectList(string pOrgId);
    Task<IEnumerable<OrgBranch>> GetOrgBranchWithOutSelf(string pOrgId, int selfBranchId);
    Task<List<SelectListItem>> GetOrgBranchSelectListItems(int orgId);
    Task<List<SelectListItem>> GetOrgBranchSelectListItems(string pOrgId);
    Task<IEnumerable<CustomSelectListItem>> GetOrgBranchSelectListByUser(string pOrgId, List<int> branchIds, bool isRequiredBranch);
}