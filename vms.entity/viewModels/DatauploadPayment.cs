namespace vms.entity.viewModels;

public class DatauploadPayment
{
    public string PaymentId { get; set; }
    public string PaymentMethodId { get; set; }
    public decimal PaymentAmount { get; set; }
    public System.DateTime PaymentDate { get; set; }
    public string PurchaseId { get; set; }
}