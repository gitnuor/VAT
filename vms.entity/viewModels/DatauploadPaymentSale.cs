namespace vms.entity.viewModels;

public class DatauploadPaymentSale
{
    public string PaymentId { get; set; }
    public string ReceivedPaymentMethodId { get; set; }
    public decimal PaymentAmount { get; set; }
    public System.DateTime PaymentDate { get; set; }
    public string SalesId { get; set; }
}