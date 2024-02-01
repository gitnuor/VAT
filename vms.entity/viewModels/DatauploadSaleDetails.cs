namespace vms.entity.viewModels;

public class DatauploadSaleDetails
{
    public string SalesDetailsID { get; set; }
    public string ProductId { get; set; }
    public string ProductVattypeId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPerItem { get; set; }
    public decimal SupplementaryDutyPercent { get; set; }
    public decimal VATPercent { get; set; }
    public string MeasurementUnitId { get; set; }
    public string SalesID { get; set; }
}