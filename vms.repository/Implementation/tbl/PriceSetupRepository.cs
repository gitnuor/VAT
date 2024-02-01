using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class PriceSetupRepository : RepositoryBase<PriceSetup>, IPriceSetupRepository
{
    private readonly DbContext _context;
    private readonly IDataProtector _dataProtector;

    public PriceSetupRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
        _context = context;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<ViewInputOutputCoEfficient>> GetInputOutputCoEfficientReportData(string pOrgId)
    {
        var id = int.Parse(_dataProtector.Unprotect(pOrgId));
        return await _context.Set<ViewInputOutputCoEfficient>()
            .Where(s => s.OrganizationId == id)
            .OrderByDescending(s => s.PriceSetupId)
            .AsNoTracking()
            .ToListAsync();
    }

}