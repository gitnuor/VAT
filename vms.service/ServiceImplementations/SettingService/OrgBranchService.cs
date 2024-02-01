using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;
using vms.service.Services.SettingService;

namespace vms.service.ServiceImplementations.SettingService;

public class OrgBranchService : ServiceBase<OrgBranch>, IOrgBranchService
{
    private readonly IOrgBranchRepository _repository;

    public OrgBranchService(IOrgBranchRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrgBranch>> GetOrgBranchList()
    {
        return await _repository.GetOrgBranch();
    }

    public async Task<IEnumerable<OrgBranch>> GetOrgBranchByOrganization(string pOrgId)
    {
        return await _repository.GetOrgBranchByOrgId(pOrgId);
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetOrgBranchSelectList(string pOrgId)
    {
        var branches = await _repository.GetOrgBranchByOrgId(pOrgId);
        return branches.ConvertToCustomSelectList(nameof(OrgBranch.OrgBranchId),
            nameof(OrgBranch.Name));
    }

    public async Task<IEnumerable<CustomSelectListItem>> GetOrgBranchSelectListByUser(string pOrgId, List<int> branchIds,
        bool isRequiredBranch)
    {
        var branches = await _repository.GetOrgBranchByOrgIdAndUser(pOrgId, branchIds, isRequiredBranch);
        return branches.Select(x => new
        {
            x.OrgBranchId,
            Name = x.Name + (string.IsNullOrEmpty(x.Code) ? "" : $" ({x.Code})")
        }).ConvertToCustomSelectList(nameof(OrgBranch.OrgBranchId),
            nameof(OrgBranch.Name));
    }

    public async Task<IEnumerable<OrgBranch>> GetOrgBranchWithOutSelf(string pOrgId, int selfBranchId)
    {
        return await _repository.GetOrgBranchWithOutSelf(pOrgId, selfBranchId);
    }

    public async Task<OrgBranch> GetOrgBranch(string idEnc)
    {
        return await _repository.GetOrgBranch(idEnc);
    }

    /*** Created by Akash only for searching criteria***/
    public async Task<List<SelectListItem>> GetOrgBranchSelectListItems(int orgId)
    {
        var orgBranchList = await _repository.Query().Where(x => x.OrganizationId == orgId && x.IsActive == true)
            .SelectAsync();
        return orgBranchList.Select(s => new SelectListItem { Text = s.Name, Value = s.OrgBranchId.ToString() })
            .ToList();
    }

    public async Task<List<SelectListItem>> GetOrgBranchSelectListItems(string pOrgId)
    {
        var branches = await _repository.GetOrgBranchByOrgId(pOrgId);
        return branches.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.OrgBranchId.ToString()
        }).ToList();
    }
}