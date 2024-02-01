using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class Sp6p6
{
    [Key]

    public string Name { get; set; }
    public string BinNo { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? VatChallanIssueDate { get; set; }
    public decimal? TotalPriceWithoutVat { get; set; }
    public string ORGNAME { get; set; }
    public string OrgAddress { get; set; }
    public string OrgBin { get; set; }

    public decimal? TotalVAT { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public string VDSCertificateNo { get; set; }
    public decimal? VDSAmount { get; set; }
    public DateTime? VDSCertificateDate { get; set; }
}