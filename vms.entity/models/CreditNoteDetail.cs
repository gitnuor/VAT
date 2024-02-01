﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class CreditNoteDetail
{
    public int CreditNoteDetailId { get; set; }

    public int CreditNoteId { get; set; }

    public int SalesDetailId { get; set; }

    public string ReasonOfReturn { get; set; }

    public decimal ReturnQuantity { get; set; }

    public int MeasurementUnitId { get; set; }

    public decimal ConversionRatio { get; set; }

    public string ReasonOfReturnInDetail { get; set; }

    public string ReferenceKey { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual CreditNote CreditNote { get; set; }

    public virtual MeasurementUnit MeasurementUnit { get; set; }

    public virtual ICollection<ProductTransactionBook> ProductTransactionBooks { get; set; } = new List<ProductTransactionBook>();

    public virtual SalesDetail SalesDetail { get; set; }
}