using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class Sp6p5
{
    [Key]
    public string PareName { get; set; }
    public string PareBin { get; set; }
    public string PareAddre { get; set; }
    public string PareVatPName { get; set; }
    public string PareVatPDes { get; set; }
    public string SenName { get; set; }
    public string SenAddre { get; set; }
    public string ReciName { get; set; }
    public string ReciAddre { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime? SalesDate { get; set; }
 
    public string ProduName { get; set; }
    public Decimal? Qty { get; set; }
    public Decimal? Price { get; set; }
    public Decimal? VtaAmount { get; set; }
}