﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class SalesDetail
{
    public int SalesDetailId { get; set; }

    public int SalesId { get; set; }

    public int ProductId { get; set; }

    public string ProductDescription { get; set; }

    public string Hscode { get; set; }

    public string ProductCode { get; set; }

    public string PartNo { get; set; }

    public string GoodsId { get; set; }

    public string Skuno { get; set; }

    public string Skuid { get; set; }

    public int ProductVattypeId { get; set; }

    public long? ProductTransactionBookId { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? ServiceChargePercent { get; set; }

    public decimal DiscountPerItem { get; set; }

    public decimal Vatpercent { get; set; }

    public decimal SupplementaryDutyPercent { get; set; }

    public decimal? TdsPercent { get; set; }

    public int MeasurementUnitId { get; set; }

    public decimal ConversionRatio { get; set; }

    public string ReferenceKey { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual ICollection<CreditNoteDetail> CreditNoteDetails { get; set; } = new List<CreditNoteDetail>();

    public virtual ICollection<DamageDetail> DamageDetails { get; set; } = new List<DamageDetail>();

    public virtual MeasurementUnit MeasurementUnit { get; set; }

    public virtual Product Product { get; set; }

    public virtual ProductTransactionBook ProductTransactionBook { get; set; }

    public virtual ICollection<ProductTransactionBook> ProductTransactionBooks { get; set; } = new List<ProductTransactionBook>();

    public virtual ProductVattype ProductVattype { get; set; }

    public virtual Sale Sales { get; set; }

    public virtual ICollection<SalesPriceAdjustmentDetail> SalesPriceAdjustmentDetails { get; set; } = new List<SalesPriceAdjustmentDetail>();
}