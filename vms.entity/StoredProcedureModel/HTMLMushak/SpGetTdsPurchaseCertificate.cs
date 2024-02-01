using System;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class SpGetTdsPurchaseCertificate
{
    public string VendorName { get; set; }
    public bool IsVendorHasTin { get; set; }
    public string IsVendorHasTinStatus { get; set; }
    public string VendorTin { get; set; }
    public string VendorBin { get; set; }
    public string VendorEmailAddress { get; set; }
    public string VendorAddress { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? VatChallanIssueDate { get; set; }
    public decimal TotalPriceWithoutVat { get; set; }
    public string OrgName { get; set; }
    public string OrgAddress { get; set; }
    public string OrgBin { get; set; }

    public decimal TotalVAT { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public string VatResponsiblePersonTcan { get; set; }
    public string VatResponsiblePersonMobileNo { get; set; }
    public string VatResponsiblePersonEmailAddress { get; set; }
    public string TdsCertificateNo { get; set; }
    public decimal? TdsAmount { get; set; }
    public DateTime? TdsCertificateDate { get; set; }
    public decimal? TdsPaidAmount { get; set; }
    public int TdsPaymentId { get; set; }
    public int OrganizationId { get; set; }
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