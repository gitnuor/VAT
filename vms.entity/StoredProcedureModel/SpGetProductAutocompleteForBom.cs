using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel;

public class SpGetProductAutocompleteForBom
{
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ModelNo { get; set; }
    public string Code { get; set; }
    public decimal MaxUseQty { get; set; }
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
}
public class SpGetProductAutocompleteForPriceSetu
{
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ModelNo { get; set; }
    public string Code { get; set; }
    public decimal MaxUseQty { get; set; }
    public decimal UnitPrice { get; set; }
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
    public bool IsApplicableAsRawMaterial { get; set; }
    public string ItemType { get; set; } 
}