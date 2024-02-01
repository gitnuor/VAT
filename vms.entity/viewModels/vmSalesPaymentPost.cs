using System;

namespace vms.entity.viewModels;

public class vmSalesPaymentPost
{
    public string SalesPaymentReceiveId { get; set; }
    public string SalesId { get; set; }
    public string ReceivedPaymentMethodId { get; set; }
    public decimal ReceiveAmount { get; set; }
    public DateTime ReceiveDate { get; set; }
}