﻿using System;
using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmSaleOrder
{
    public List<SalesDetail> SalesDetailList { get; set; }
    public List<Content> ContentInfoJson { get; set; }
    public List<SalesPaymentReceive> SalesPaymentReceiveJson { get; set; }
    public int SalesId { get; set; }
    public string InvoiceNo { get; set; }
    public string VatChallanNo { get; set; }
    public int OrganizationId { get; set; }
    public int NoOfIteams { get; set; }
    public decimal TotalPriceWithoutVat { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }
    public decimal TotalDiscountOnIndividualProduct { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalSupplimentaryDuty { get; set; }
    public bool IsVatDeductedInSource { get; set; }
    public decimal? ReceivableAmount { get; set; }
    public decimal? PaymentReceiveAmount { get; set; }
    public decimal? PaymentDueAmount { get; set; }
    public int? CustomerId { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverContactNo { get; set; }
    public string ShippingAddress { get; set; }
    public int? ShippingCountryId { get; set; }
    public int SalesTypeId { get; set; }
    public int SalesDeliveryTypeId { get; set; }
    public string WorkOrderNo { get; set; }
    public int VehicleTypeId { get; set; }
    public string VehicleName { get; set; }
    public string VehicleRegNo { get; set; }
    public string VehicleDriverName { get; set; }
    public string VehicleDriverContactNo { get; set; }
    public DateTime SalesDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? DeliveryMethodId { get; set; }
    public int? ExportTypeId { get; set; }
    public string LcNo { get; set; }
    public DateTime? LcDate { get; set; }
    public string BillOfEntry { get; set; }
    public DateTime? BillOfEntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string TermsOfLc { get; set; }
    public string CustomerPoNumber { get; set; }
    public bool IsComplete { get; set; }
    public bool? IsTaxInvoicePrined { get; set; }
    public int? MushakGenerationId { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public int Flag { get; set; }
    public decimal VDSAmount { get; set; }
    public string FileName { get; set; }
    public int? OtherBranchOrganizationId { get; set; }
    public DateTime? TaxInvoicePrintedTime { get; set; }
    public string ReferenceKey { get; set; }
}