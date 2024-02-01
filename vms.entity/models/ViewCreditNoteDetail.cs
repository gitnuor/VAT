﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ViewCreditNoteDetail
{
    public int SalesDetailId { get; set; }

    public int SalesId { get; set; }

    public int OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public string OrganizationAddress { get; set; }

    public string OrganizationBin { get; set; }

    public int? CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string CustomerBin { get; set; }

    public DateTime? CreditNoteCreatedTime { get; set; }

    public string CreditNoteChallanNo { get; set; }

    public DateTime ReturnDate { get; set; }

    public DateTime? CreditNoteChallanPrintTime { get; set; }

    public string ReasonOfReturn { get; set; }

    public decimal ReturnQuantity { get; set; }

    public int CreditNoteMeasurementUnitId { get; set; }

    public string CreditNoteMeasurementUnitName { get; set; }

    public int? BranchId { get; set; }

    public int SalesTypeId { get; set; }

    public string SalesTypeName { get; set; }

    public string VatChallanNo { get; set; }

    public DateTime SalesDate { get; set; }

    public DateTime? SalesCreatedTime { get; set; }

    public string ShippingAddress { get; set; }

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

    public string HsCode { get; set; }

    public string ProductCode { get; set; }

    public string PartNo { get; set; }

    public string GoodsId { get; set; }

    public string SkuNo { get; set; }

    public string SkuId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int VatTypeId { get; set; }

    public string VatTypeName { get; set; }

    public decimal VatPercent { get; set; }

    public decimal? VatAmount { get; set; }

    public decimal SupplementaryDutyPercent { get; set; }

    public decimal? SupplementaryDuty { get; set; }

    public int MeasurementUnitId { get; set; }

    public string MeasurementUnitName { get; set; }

    public string ReferenceKey { get; set; }

    public DateTime? CreatedTime { get; set; }
}