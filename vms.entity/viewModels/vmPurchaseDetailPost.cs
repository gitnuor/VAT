namespace vms.entity.viewModels;

public class vmPurchaseDetailPost
{
    public string PurchaseDetailId { get; set; }
    public string PurchaseId { get; set; }
    public string ProductId { get; set; }
    public string ProductVattypeId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPerItem { get; set; }
    public decimal CustomDutyPercent { get; set; }
    public decimal ImportDutyPercent { get; set; }
    public decimal RegulatoryDutyPercent { get; set; }
    public decimal SupplementaryDutyPercent { get; set; }
    public decimal Vatpercent { get; set; }
    public decimal AdvanceTaxPercent { get; set; }
    public decimal AdvanceIncomeTaxPercent { get; set; }
    public string MeasurementUnitId { get; set; }
}