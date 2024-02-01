using System;

namespace vms.entity.viewModels;

public class vmPurchasePaymentPost
{
    public string PurchasePaymentId { get; set; }
    public string PurchaseId { get; set; }
    public string PaymentMethodId { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
}