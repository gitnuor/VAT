using System.Collections.Generic;

namespace vms.entity.viewModels;

public class DataimportPurchaseFinal
{
    public List<DatauploadPurshase> purchase { get; set; }
    public List<DatauploadPurshaseDetails> purchaseDetails { get; set; }
    public List<DatauploadPayment> Payments { get; set; }
}