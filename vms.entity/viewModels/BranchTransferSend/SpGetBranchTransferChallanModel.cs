using System.Collections.Generic;

namespace vms.entity.viewModels.BranchTransferSend;

public class SpGetBranchTransferChallanModel
{
	public SpGetBranchTransferChallanMainModel TransferChallan { get; set; }
	public IEnumerable<SpGetBranchTransferChallanDetailModel> TransferChallanDetails { get; set; }
}