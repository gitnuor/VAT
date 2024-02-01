using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class DebitNoteRepository : RepositoryBase<DebitNote>, IDebitNoteRepository
{
    private readonly DbContext _context;
    private readonly IDataProtector _dataProtector;

    public DebitNoteRepository(DbContext context, IDataProtectionProvider protectionProvider,
        PurposeStringConstants purposeStringConstants) : base(context)
    {
        _context = context;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<ViewDebitNote>> GetDebitNoteData(string orgIdEnc)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewDebitNote>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.DebitNoteReturnDate)
            .AsNoTracking()
            .ToListAsync();
        return list;
    }

    public async Task<IEnumerable<ViewDebitNote>> GetDebitNoteDataByOrgAndBranch(string orgIdEnc, List<int> branchIds, bool isRequiredBranch)
    {
        var orgId = int.Parse(_dataProtector.Unprotect(orgIdEnc));
        var list = await _context.Set<ViewDebitNote>()
            .Where(s => s.OrganizationId == orgId)
            .OrderByDescending(s => s.DebitNoteReturnDate)
            .AsNoTracking()
            .ToListAsync();
        if (isRequiredBranch)
        {
            list = list.Where(s => branchIds.Contains(s.BranchId.Value)).ToList();
        }
        return list;
    }

}