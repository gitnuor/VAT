using System;

namespace vms.entity.Dto.PurchaseImport;

public class PurchaseImportTaxPaymentPostWithPurchaseDto
{
	public string TaxPaymentId { get; set; }
	public string TaxPaymentTypeId { get; set; }
	public string TaxPaymentTypeName { get; set; }
	public string TaxPaymentVatCommissionerateId { get; set; }
	public string TaxPaymentVatCommissionerateName { get; set; }
	public string TaxPaymentBankId { get; set; }
	public string TaxPaymentBankName { get; set; }
	public string TaxPaymentBankBranchName { get; set; }
	public int TaxPaymentBankBranchDistrictId { get; set; }
	public string TaxPaymentBankBranchDistrictName { get; set; }
	public string TaxPaymentAccCode { get; set; }
	public string TaxPaymentDocOrChallanNo { get; set; }
	public DateTime TaxPaymentDocOrChallanDate { get; set; }
	public DateTime TaxPaymentPaymentDate { get; set; }
	public decimal TaxPaymentPaidAmount { get; set; }
	public string TaxPaymentRemarks { get; set; }
}