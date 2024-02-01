using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
{
    private readonly DbContext _context;
    private readonly IDataProtectionProvider _protectionProvider;
    private readonly PurposeStringConstants _purposeStringConstants;
    private IDataProtector _dataProtector;

    public OrganizationRepository(DbContext context, IDataProtectionProvider p_protectionProvider, PurposeStringConstants p_purposeStringConstants) : base(context)
    {
        _context = context;
        _protectionProvider = p_protectionProvider;
        _purposeStringConstants = p_purposeStringConstants;
        _dataProtector = _protectionProvider.CreateProtector(_purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<Organization>> GetOrganizations(int orgIdEnc)
    {
        var organizations = await Query().Where(w => w.OrganizationId == orgIdEnc).SelectAsync();

        organizations.ToList().ForEach(delegate (Organization organization)
        {
            organization.EncryptedId = _dataProtector.Protect(organization.OrganizationId.ToString());
        });
        return organizations;
    }
    public async Task<Organization> GetOrganization(string idEnc)
    {
        int id = int.Parse(_dataProtector.Unprotect(idEnc));
        var organization = await Query()
            .SingleOrDefaultAsync(x => x.OrganizationId == id, System.Threading.CancellationToken.None);
        organization.EncryptedId = _dataProtector.Protect(organization.OrganizationId.ToString());

        return organization;
    }

    public async Task<IEnumerable<Organization>> GetParentOrganizations(int par_orgId)
    {
        var organizations = await Query().Where(w => w.ParentOrganizationId == par_orgId).SelectAsync();

        organizations.ToList().ForEach(delegate (Organization organization)
        {
            organization.EncryptedId = _dataProtector.Protect(organization.OrganizationId.ToString());
        });
        return organizations;

    }
}