﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int OrganizationId { get; set; }

    public int? OrgBranchId { get; set; }

    public int PurchaseSlNo { get; set; }

    public int? TransferSalesId { get; set; }

    public int? TransferBranchId { get; set; }

    public int? VendorId { get; set; }

    public string VendorName { get; set; }

    public string VendorBin { get; set; }

    public string VendorNid { get; set; }

    public string VendorAddress { get; set; }

    public string VendorContactNo { get; set; }

    public string VatChallanNo { get; set; }

    public DateTime? VatChallanIssueDate { get; set; }

    public string VendorInvoiceNo { get; set; }

    public string InvoiceNo { get; set; }

    public string VoucherNo { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int PurchaseTypeId { get; set; }

    public int PurchaseReasonId { get; set; }

    public int NoOfIteams { get; set; }

    public decimal TotalPriceWithoutVat { get; set; }

    public decimal DiscountOnTotalPrice { get; set; }

    public decimal TotalDiscountOnIndividualProduct { get; set; }

    public decimal TotalCustomDuty { get; set; }

    public decimal TotalImportDuty { get; set; }

    public decimal TotalRegulatoryDuty { get; set; }

    public decimal TotalSupplementaryDuty { get; set; }

    public decimal TotalVat { get; set; }

    public decimal TotalAdvanceTax { get; set; }

    public decimal TotalAdvanceIncomeTax { get; set; }

    public decimal? AdvanceTaxPaidAmount { get; set; }

    public DateTime? Atpdate { get; set; }

    public int? AtpbankId { get; set; }

    public string AtpbankBranchName { get; set; }

    public int? AtpnbrEconomicCodeId { get; set; }

    public string AtpchallanNo { get; set; }

    public bool IsVatDeductedInSource { get; set; }

    public DateTime? Vdsdate { get; set; }

    public decimal? Vdsamount { get; set; }

    public bool? IsVdsamountPaid { get; set; }

    public bool? IsVdscertificatePrinted { get; set; }

    public string VdscertificateNo { get; set; }

    public DateTime? VdscertificateDate { get; set; }

    public string VdspaymentBookTransferNo { get; set; }

    public string Vdsnote { get; set; }

    public bool? IsTds { get; set; }

    public DateTime? TdsDate { get; set; }

    public decimal? TdsAmount { get; set; }

    public bool? IsTdsAmountPaid { get; set; }

    public bool? IsTdsCertificatePrinted { get; set; }

    public string TdsCertificateNo { get; set; }

    public DateTime? TdsCertificateDate { get; set; }

    public string TdsPaymentBookTransferNo { get; set; }

    public string TdsNote { get; set; }

    public decimal? PayableAmount { get; set; }

    public decimal? PaidAmount { get; set; }

    public decimal? DueAmount { get; set; }

    public DateTime? ExpectedDeliveryDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string LcNo { get; set; }

    public DateTime? LcDate { get; set; }

    public string LcaNumber { get; set; }

    public string LcafNumber { get; set; }

    public string BillOfEntry { get; set; }

    public DateTime? BillOfEntryDate { get; set; }

    public int? CustomsAndVatcommissionarateId { get; set; }

    public DateTime? DueDate { get; set; }

    public string TermsOfLc { get; set; }

    public string PoNumber { get; set; }

    public int? MushakGenerationId { get; set; }

    public string ReferenceKey { get; set; }

    public bool IsComplete { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public string PurchaseRemarks { get; set; }

    public long? ApiTransactionId { get; set; }

    public long? ExcelDataUploadId { get; set; }

    public bool? IsApproved { get; set; }

    public int? ApprovedBy { get; set; }

    public string ApproveMessage { get; set; }

    public bool? IsRejected { get; set; }

    public int? RejectedBy { get; set; }

    public string RejectMessage { get; set; }

    public virtual Bank Atpbank { get; set; }

    public virtual NbrEconomicCode AtpnbrEconomicCode { get; set; }

    public virtual CustomsAndVatcommissionarate CustomsAndVatcommissionarate { get; set; }

    public virtual ICollection<Damage> Damages { get; set; } = new List<Damage>();

    public virtual ICollection<DebitNote> DebitNotes { get; set; } = new List<DebitNote>();

    public virtual ICollection<MushakReturnPaymentForVd> MushakReturnPaymentForVds { get; set; } = new List<MushakReturnPaymentForVd>();

    public virtual OrgBranch OrgBranch { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<PurchaseImportTaxPayment> PurchaseImportTaxPayments { get; set; } = new List<PurchaseImportTaxPayment>();

    public virtual ICollection<PurchasePayment> PurchasePayments { get; set; } = new List<PurchasePayment>();

    public virtual PurchaseReason PurchaseReason { get; set; }

    public virtual PurchaseType PurchaseType { get; set; }

    public virtual ICollection<TdsPaymentForPurchase> TdsPaymentForPurchases { get; set; } = new List<TdsPaymentForPurchase>();

    public virtual Organization TransferBranch { get; set; }

    public virtual Sale TransferSales { get; set; }

    public virtual Vendor Vendor { get; set; }
}