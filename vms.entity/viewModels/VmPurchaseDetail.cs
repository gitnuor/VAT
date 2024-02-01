using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class VmPurchaseDetail
{
    public Purchase Purchase { get; set; }
    public IEnumerable<PurchaseDetail> PurchaseDetails { get; set; }
    public IEnumerable<DebitNote> DebitNotes { get; set; }
}