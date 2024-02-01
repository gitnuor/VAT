using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels.BranchTransferSend;

public class BranchTransferSendDetailViewModel
{
    public models.BranchTransferSend BranchTransferSend { get; set; }
    public IEnumerable<BranchTransferSendDetail> BranchTransferSendDetails { get; set; }
}