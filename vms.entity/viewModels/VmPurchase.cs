﻿using System;
using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class VmPurchase
{
    public List<PurchaseDetail> PurchaseOrderDetailList { get; set; }
    public List<Content> ContentInfoJson { get; set; }
    public List<PurchasePayment> PurchasePaymenJson { get; set; }
    public int PurchaseId { get; set; }
    public int OrganizationId { get; set; }
    public int VendorId { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? VatChallanIssueDate { get; set; }
    public string VendorInvoiceNo { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int PurchaseTypeId { get; set; }
    public int PurchaseReasonId { get; set; }
    public int NoOfIteams { get; set; }
    public decimal TotalPriceWithoutVat { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }
    public decimal TotalDiscountOnIndividualProduct { get; set; }
    public decimal TotalImportDuty { get; set; }
    public decimal TotalRegulatoryDuty { get; set; }
    public decimal TotalSupplementaryDuty { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalAdvanceTax { get; set; }
    public decimal TotalAdvanceIncomeTax { get; set; }
    public bool IsVatDeductedInSource { get; set; }
    public decimal? PayableAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? DueAmount { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string LcNo { get; set; }
    public DateTime? LcDate { get; set; }
    public string BillOfEntry { get; set; }
    public DateTime? BillOfEntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string TermsOfLc { get; set; }
    public string PoNumber { get; set; }
    public int? MushakGenerationId { get; set; }
    public bool IsComplete { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public int Flag { get; set; }
    public string FileName { get; set; }
    public decimal? VDSAmount { get; set; }
    public decimal? AdvanceTaxPaidAmount { get; set; }
    public DateTime? ATPDate { get; set; }
    public int? ATPBankBranchId { get; set; }
    public string ATPBankBranchName { get; set; }
    public int? ATPNbrEconomicCodeId { get; set; }
    public string ATPChallanNo { get; set; }
    public int? CustomsAndVATCommissionarateId { get; set; }
    public string ReferenceKey { get; set; }
}