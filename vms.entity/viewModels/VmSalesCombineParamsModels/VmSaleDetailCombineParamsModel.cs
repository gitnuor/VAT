namespace vms.entity.viewModels.VmSalesCombineParamsModels;

public class VmSaleDetailCombineParamsModel
{
    public int SalesDetailId { get; set; }
    public int SalesId { get; set; }
    public int ProductId { get; set; }
    public string ProductDescription { get; set; }
    public string Hscode { get; set; }
    public string ProductCode { get; set; }
    public string SKUNo { get; set; }
    public string SKUId { get; set; }
    public string GoodsId { get; set; }
    public int ProductVattypeId { get; set; }
    public long? ProductTransactionBookId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ServiceChargePercent { get; set; }
    public decimal DiscountPerItem { get; set; }
    public decimal Vatpercent { get; set; }
    public decimal SupplementaryDutyPercent { get; set; }
    public int MeasurementUnitId { get; set; }
    public string ReferenceKey { get; set; }
}