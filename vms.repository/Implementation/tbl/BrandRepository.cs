using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
{
    private readonly DbContext _context;
    private readonly IDataProtector _dataProtector;

    public BrandRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
        _context = context;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<Brand>> GetBrandsByOrg(string pOrgId)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(pOrgId));
        return await Query()
            .Where(o => o.OrganizationId == orgId)
            .SelectAsync();
    }
}