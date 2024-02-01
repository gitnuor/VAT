using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class BranchTransferReceiveRepository : RepositoryBase<BranchTransferReceive>, IBranchTransferReceiveRepository
{
    private readonly DbContext _context;
    private readonly IDataProtector _dataProtector;

    public BranchTransferReceiveRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
        _context = context;
        _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<BranchTransferReceive>> GetBranchTransferReceivesByOrganization(string orgIdEnc)
	{
		var id = int.Parse(_dataProtector.Unprotect(orgIdEnc));
		var branchTransferReceives = await Query()
			.Where(a => a.OrganizationId == id)
			.Include(a => a.OrgBranchSender)
			.Include(a => a.OrgBranchReceiver)
			.SelectAsync();
		var list = branchTransferReceives.ToList();
		list.ForEach(delegate (BranchTransferReceive BranchTransferReceive)
		{
			BranchTransferReceive.EncryptedId = _dataProtector.Protect(BranchTransferReceive.BranchTransferReceiveId.ToString());
		});
		return list;
	}
}