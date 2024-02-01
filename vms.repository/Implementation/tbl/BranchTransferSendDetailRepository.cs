using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using vms.entity.models;
using vms.entity.viewModels;
using vms.repository.Repository.tbl;

namespace vms.repository.Implementation.tbl;

public class BranchTransferSendDetailRepository : RepositoryBase<BranchTransferSendDetail>, IBranchTransferSendDetailRepository
{
	private readonly IDataProtector _dataProtector;

    public BranchTransferSendDetailRepository(DbContext context, IDataProtectionProvider protectionProvider, PurposeStringConstants purposeStringConstants) : base(context)
    {
	    _dataProtector = protectionProvider.CreateProtector(purposeStringConstants.UserIdQueryString);
    }

    public async Task<IEnumerable<BranchTransferSendDetail>> GetBranchTransferSendDetailsByBranchTransferSend(string branchTransferSendIdEnc)
	{
		var id = int.Parse(_dataProtector.Unprotect(branchTransferSendIdEnc));
		var branchTransferSendDetails = await Query()
			.Where(a => a.BranchTransferSendId == id)
			.Include(a => a.Product)
			.Include(a => a.MeasurementUnit)
			.SelectAsync();
		var list = branchTransferSendDetails.ToList();
		list.ForEach(delegate (BranchTransferSendDetail branchTransferSendDetail)
		{
			branchTransferSendDetail.EncryptedId = _dataProtector.Protect(branchTransferSendDetail.BranchTransferSendDetailId.ToString());
		});
		return list;
	}
}