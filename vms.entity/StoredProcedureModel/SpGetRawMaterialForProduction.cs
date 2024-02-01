namespace vms.entity.StoredProcedureModel;

public class SpGetRawMaterialForProduction
{
    public int Id  { get; set; }
    public string ProdName { get; set; }
    public string ProductDescription { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal? RequiredQtyPerUnitProduction { get; set; }
    public int MeasurementUnitId { get; set; }
    public string UnitName { get; set; }

}