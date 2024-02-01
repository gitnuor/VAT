using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class SpDebitMushak
{
    [Key]

    public string VENName { get; set; }
    public string VENBin { get; set; }
    public string OrgName { get; set; }
    public DateTime? PSDate { get; set; }
    public string PSInvoice { get; set; }
    public string OrgBin { get; set; }
    public string VatPName { get; set; }
    public string VatPDes { get; set; }
    public int? DebitNo { get; set; }
    public DateTime? DrTime { get; set; }
    public string ProductName { get; set; }
    public string Quantity { get; set; }
    public decimal? ReturnQuantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public string ReasonOfReturn { get; set; }
    public decimal? SupplementaryDutyAmount { get; set; }
    public decimal? VatAmount { get; set; }
    public decimal? DeductionAmount { get; set; }
    public decimal? TotalAmount { get; set; }
}