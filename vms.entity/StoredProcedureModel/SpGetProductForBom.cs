namespace vms.entity.StoredProcedureModel;

public class SpGetProductForBom
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ModelNo { get; set; }
    public string ProductCode { get; set; }
    public decimal MaxUseQty { get; set; }
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
}