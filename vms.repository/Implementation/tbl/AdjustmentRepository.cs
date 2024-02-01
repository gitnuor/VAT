using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class AdjustmentRepository : RepositoryBase<Adjustment>, IAdjustmentRepository
{
    private readonly DbContext _context;
    private readonly IDataProtector _dataProtector;

    public AdjustmentRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
        _context = context;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<Adjustment>> GetByOrg(string pOrgId)
    {
        var id = int.Parse(_dataProtector.Unprotect(pOrgId));
        var adjustments = await Query()
            .Where(a => a.OrganizationId == id)
            .Include(a => a.AdjustmentType)
            .SelectAsync();
        var adjustmentList = adjustments.ToList();
        adjustmentList.ForEach(delegate(Adjustment adjustment)
        {
            adjustment.EncryptedId = _dataProtector.Protect(adjustment.AdjustmentId.ToString());
        });
        return adjustmentList;
    }
}