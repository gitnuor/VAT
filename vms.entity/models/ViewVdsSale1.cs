﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ViewVdsSale1
{
    public int SalesId { get; set; }

    public int SalesTypeId { get; set; }

    public string SalesTypeName { get; set; }

    public int OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public int? OrgBranchId { get; set; }

    public string BranchName { get; set; }

    public DateTime SalesDate { get; set; }

    public string InvoiceNo { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public string VatChallanNo { get; set; }

    public decimal TotalPriceWithoutVat { get; set; }

    public bool? IsVdscertificateReceived { get; set; }

    public bool IsVatChallanPrinted { get; set; }

    public string IsVatChallanPrintedStatus { get; set; }

    public DateTime? VatChallanPrintedTime { get; set; }

    public int? CustomerId { get; set; }

    public string CustomerName { get; set; }

    public decimal? TotalPriceWithoutTax { get; set; }

    public decimal? TotalSupplementaryDuty { get; set; }

    public decimal? TotalVat { get; set; }

    public bool IsVds { get; set; }

    public string IsVdsStatus { get; set; }

    public decimal? Vdsamount { get; set; }

    public bool? IsTds { get; set; }

    public string IsTdsStatus { get; set; }

    public decimal? TdsAmount { get; set; }

    public decimal? TotalReceivableAmount { get; set; }

    public bool IsComplete { get; set; }

    public string IsCompleteStatus { get; set; }

    public bool? IsApproved { get; set; }

    public string IsApprovedStatus { get; set; }

    public bool? IsRejected { get; set; }

    public string IsRejectedStatus { get; set; }

    public string ApproveStatus { get; set; }
}