namespace vms.entity.StoredProcedureModel;

public class SpGetProductForSale
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string HSCode { get; set; }
    public string ModelNo { get; set; }
    public string ProductCode { get; set; }
    public decimal SalesUnitPrice { get; set; }
    public int ProductVatTypeId { get; set; }
    public decimal DefaultVatPercent { get; set; }
    public decimal SupplementaryDutyPercent { get; set; }
    public decimal MaxSaleQty { get; set; }
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
    public bool IsInventory { get; set; }
    public bool IsMeasurable { get; set; }
}