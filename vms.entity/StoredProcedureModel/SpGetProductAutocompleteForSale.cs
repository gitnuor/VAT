using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel;

public class SpGetProductAutocompleteForSale
{
    [Key]
    public int ProductId { get; set; }

    public string ProductName { get; set; }
    public string ModelNo { get; set; }
    public string Code { get; set; }
    public decimal SalesUnitPrice { get; set; }
    public decimal DefaultVatPercent { get; set; }
    public decimal SupplimentaryDutyPercent { get; set; }
    public decimal MaxSaleQty { get; set; }
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
    public int ProductVATTypeId { get; set; }
}