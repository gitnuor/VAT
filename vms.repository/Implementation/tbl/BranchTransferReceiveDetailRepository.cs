using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class BranchTransferReceiveDetailRepository : RepositoryBase<BranchTransferReceiveDetail>, IBranchTransferReceiveDetailRepository
{
	private readonly IDataProtector _dataProtector;

    public BranchTransferReceiveDetailRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
	    _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<BranchTransferReceiveDetail>> GetBranchTransferReceiveDetailsByBranchTransferReceive(string branchTransferReceiveIdEnc)
	{
		var id = int.Parse(_dataProtector.Unprotect(branchTransferReceiveIdEnc));
		var branchTransferReceiveDetails = await Query()
			.Where(a => a.BranchTransferReceiveId == id)
			.Include(a => a.Product)
			.Include(a => a.MeasurementUnit)
			.SelectAsync();
		var list = branchTransferReceiveDetails.ToList();
		list.ForEach(delegate (BranchTransferReceiveDetail branchTransferReceiveDetail)
		{
			branchTransferReceiveDetail.EncryptedId = _dataProtector.Protect(branchTransferReceiveDetail.BranchTransferReceiveDetailId.ToString());
		});
		return list;
	}
}