namespace vms.entity.viewModels.SalesPriceAdjustment;

public class SalesPriceAdjustmentCombineDetailParamsViewModel
{
    public int SalesDetailId { get; set; }
    public int SalesId { get; set; }
    public decimal QuantityToChange { get; set; }
    public decimal ChangeAmountPerItem { get; set; }
    public int MeasurementUnitId { get; set; }
    public string ReasonOfChange { get; set; }
    public string ReferenceKey { get; set; }
}