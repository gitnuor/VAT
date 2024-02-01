﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class BranchTransferReceiveDetail
{
    public int BranchTransferReceiveDetailId { get; set; }

    public int BranchTransferSendDetailId { get; set; }

    public int BranchTransferReceiveId { get; set; }

    public int ProductId { get; set; }

    public string ProductDescription { get; set; }

    public string Hscode { get; set; }

    public string ProductCode { get; set; }

    public string PartNo { get; set; }

    public string GoodsId { get; set; }

    public string Skuno { get; set; }

    public string Skuid { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int MeasurementUnitId { get; set; }

    public decimal ConversionRatio { get; set; }

    public string ReferenceKey { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual BranchTransferReceive BranchTransferReceive { get; set; }

    public virtual BranchTransferSendDetail BranchTransferSendDetail { get; set; }

    public virtual MeasurementUnit MeasurementUnit { get; set; }

    public virtual Product Product { get; set; }

    public virtual ICollection<ProductTransactionBook> ProductTransactionBooks { get; set; } = new List<ProductTransactionBook>();
}