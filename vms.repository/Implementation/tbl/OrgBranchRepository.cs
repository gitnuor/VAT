using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class OrgBranchRepository : RepositoryBase<OrgBranch>, IOrgBranchRepository
{
    private readonly DbContext _context;
    private readonly IDataProtector _dataProtector;

    public OrgBranchRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
        _context = context;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<OrgBranch>> GetOrgBranchByOrgId(string pOrgId)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
        return await Query()
            .Where(o => o.OrganizationId == orgId)
            .SelectAsync();
    }

    public async Task<IEnumerable<OrgBranch>> GetOrgBranchByOrgIdAndUser(string pOrgId, List<int> branchIds, bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
        var list = await Query()
            .Where(o => o.OrganizationId == orgId)
            .SelectAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.OrgBranchId)).ToList();
        }
        return list;
    }

    public async Task<IEnumerable<OrgBranch>> GetOrgBranchWithOutSelf(string pOrgId,int selfBranchId)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
        return await Query()
            .Where(o => o.OrganizationId == orgId)
            .Where(o => o.OrgBranchId == selfBranchId)
            .SelectAsync();
    }


    public async Task<IEnumerable<OrgBranch>> GetOrgBranch()
    {
        return await Query()
            .SelectAsync();
    }


    public async Task<OrgBranch> GetOrgBranch(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var organization = await Query()
            .SingleOrDefaultAsync(x => x.OrgBranchId == id, System.Threading.CancellationToken.None);
        organization.EncryptedId = _dataProtector.Protect(organization.OrgBranchId.ToString());

        return organization;
    }
}