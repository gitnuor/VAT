using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels.BranchTransferReceive;

public class BranchTransferReceiveDetailViewModel
{
    public models.BranchTransferReceive BranchTransferReceive { get; set; }
    public IEnumerable<BranchTransferReceiveDetail> BranchTransferReceiveDetails { get; set; }
}