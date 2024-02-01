namespace vms.entity.StoredProcedureModel;

public class SpGetProductForPurchase
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public string HsCode { get; set; }
    public int ProductVatTypeId { get; set; }
    public decimal DefaultVatPercent { get; set; }
    public decimal DefaultCustomDutyPercent { get; set; }
    public decimal DefaultImportDutyPercent { get; set; }
    public decimal DefaultRegulatoryDutyPercent { get; set; }
    public decimal DefaultSupplementaryDutyPercent { get; set; }
    public decimal DefaultAdvanceTaxPercent { get; set; }
    public decimal DefaultAdvanceIncomeTaxPercent { get; set; }
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
    public bool IsInventory { get; set; }
}