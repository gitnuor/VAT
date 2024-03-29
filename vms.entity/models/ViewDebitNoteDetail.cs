﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ViewDebitNoteDetail
{
    public string OrganizationName { get; set; }

    public string OrganizationAddress { get; set; }

    public string OrganizationBin { get; set; }

    public DateTime? DebitNoteCreatedTime { get; set; }

    public string DebitNoteChallanNo { get; set; }

    public string ReasonOfReturn { get; set; }

    public DateTime DebitNoteReturnDate { get; set; }

    public DateTime? DebitNoteChallanPrintTime { get; set; }

    public decimal ReturnQuantity { get; set; }

    public int DebitNoteMeasurementUnitId { get; set; }

    public string DebitNoteMeasurementUnitName { get; set; }

    public int VendorId { get; set; }

    public string VendorName { get; set; }

    public string VendorBinNo { get; set; }

    public int PurchaseDetailId { get; set; }

    public int PurchaseId { get; set; }

    public int PurchaseTypeId { get; set; }

    public string PurchaseTypeName { get; set; }

    public int OrganizationId { get; set; }

    public int? BranchId { get; set; }

    public string VatChallanNo { get; set; }

    public DateTime? VatChallanIssueDate { get; set; }

    public DateTime? PurchaseCreatedTime { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public string Code { get; set; }

    public string ModelNo { get; set; }

    public string Color { get; set; }

    public string Size { get; set; }

    public int? ProductCategoryId { get; set; }

    public string ProductCategory { get; set; }

    public int ProductTypeId { get; set; }

    public string ProductType { get; set; }

    public int? BrandId { get; set; }

    public string BrandName { get; set; }

    public string ProductDescription { get; set; }

    public string Hscode { get; set; }

    public string ProductCode { get; set; }

    public string PartNo { get; set; }

    public string GoodsId { get; set; }

    public string SkuNo { get; set; }

    public string SkuId { get; set; }

    public int VatTypeId { get; set; }

    public string VatTypeName { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal VatPercent { get; set; }

    public decimal VatAmount { get; set; }

    public decimal TotalPriceWithoutTax { get; set; }

    public decimal CustomDutyAmount { get; set; }

    public decimal ImportDutyAmount { get; set; }

    public decimal RegulatoryDutyAmount { get; set; }

    public decimal AdvanceIncomeTaxAmount { get; set; }

    public decimal SupplementaryDutyAmount { get; set; }

    public decimal AdvanceTaxAmount { get; set; }

    public decimal VatOrAtImposablePrice { get; set; }

    public int MeasurementUnitId { get; set; }

    public string MeasurementUnitName { get; set; }

    public string ReferenceKey { get; set; }

    public DateTime? CreatedTime { get; set; }
}