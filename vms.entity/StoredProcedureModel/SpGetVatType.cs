using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel;

public class SpGetVatType
{
    [Key]
    public int ProductVATTypeId { get; set; }
    public string VatTypeName { get; set; }
    public string VatTypeNameInBn { get; set; }
    public decimal DefaultVatPercent { get; set; }
}