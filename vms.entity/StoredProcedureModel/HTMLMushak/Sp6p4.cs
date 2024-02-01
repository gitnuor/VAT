using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class Sp6p4
{
    [Key]

    public string OrgName { get; set; }
    public string OrgBin { get; set; }
    public string OrgAddress { get; set; }
    public string ChallanNo { get; set; }
    public DateTime? ChallanIssueDate { get; set; }
    public string VenName { get; set; }
    public string VenBin { get; set; }
    public string VenAddress { get; set; }
    public string MesurementName { get; set; }
    public string ProductName { get; set; }
    public Decimal Quantity { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
}