using System;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class SpVdsPurchaseCertificate
{
    public string VendorName { get; set; }
    public string VendorBin { get; set; }
    public string VendorEmailAddress { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? VatChallanIssueDate { get; set; }
    public decimal TotalPriceWithoutVat { get; set; }
    public string OrgName { get; set; }
    public string OrgAddress { get; set; }
    public string OrgBin { get; set; }

    public decimal TotalVAT { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public string VatResponsiblePersonSignUrl { get; set; }
    public string VDSCertificateNo { get; set; }
    public decimal? VDSAmount { get; set; }
    public DateTime? VDSCertificateDate { get; set; }
    public decimal? VdsPaidAmount { get; set; }
    public int MushakReturnPaymentId { get; set; }
    public int CustomsAndVATCommissionarateId { get; set; }
    public decimal? PaidAmount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int BankId { get; set; }
    public string BankName { get; set; }
    public string BankBranchName { get; set; }
    public int? BankBranchDistrictOrCityId { get; set; }
    public string BankBranchDistrictName { get; set; }
    public string EconomicCode { get; set; }
    public string TreasuryChallanNo { get; set; }
    public DateTime? SubimissionDate { get; set; }
}