using System;
using System.Collections.Generic;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.entity.viewModels;

public partial class vmMushakReturnRefund:VmMushakReturnDisplay
{
    public int MushakReturnRefundId { get; set; }
    public int OrganizationId { get; set; }
    public int MushakYear { get; set; }
    public int MushakMonth { get; set; }
    public bool IsInterestedToGetRefund { get; set; }
    public decimal? InterestedToRefundVatamount { get; set; }
    public decimal? InterestedToRefundSdamount { get; set; }
    public decimal? RefundedVatamount { get; set; }
    public string RefundedVatchequeNo { get; set; }
    public DateTime? RefundedVatchequeDate { get; set; }
    public decimal? RefundedSdmount { get; set; }
    public string RefundedSdchequeNo { get; set; }
    public DateTime? RefundedSdchequeDate { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }

    public IEnumerable<CustomSelectListItem> CustomsAndVATCommissionarate;
    public IEnumerable<CustomSelectListItem> paymentType;
    public IEnumerable<CustomSelectListItem> BrankBranch;
}