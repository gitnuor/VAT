namespace vms.entity.viewModels;

public class vmSalesDetailPost
{
    public string SalesDetailId { get; set; }
    public string SalesId { get; set; }
    public string ProductId { get; set; }
    public string ProductVattypeId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPerItem { get; set; }
    public decimal Vatpercent { get; set; }
    public decimal SupplementaryDutyPercent { get; set; }
    public string MeasurementUnitId { get; set; }
}