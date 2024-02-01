using System;

namespace vms.entity.viewModels.ReportsViewModel;

public class VMMushak4P4
{
    public string Name { get; set; }
    public string Bin { get; set; }
    public string Address { get; set; }
    public string HsCode { get; set; }
    public string ProductName { get; set; }
    public Decimal Quantity { get; set; }
    public float ActualPrice { get; set; }
    public float OfferedPrice { get; set; }
    public string ReasonOfUselessness { get; set; }
    public DateTime Date { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public string VatResponsiblePersonSignature { get; set; }
    public string VatResponsiblePersonSeal { get; set; }
}