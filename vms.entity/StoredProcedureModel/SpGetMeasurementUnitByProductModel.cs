namespace vms.entity.StoredProcedureModel;

public class SpGetMeasurementUnitByProductModel
{
	public int ProductId { get; set; }
	public int MeasurementUnitId { get; set; }
	public decimal ConvertionRatio { get; set; }
	public string MeasurementUnitName { get; set; }
}
