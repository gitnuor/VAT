﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ContractualProductionTransferRawMaterialDetail
{
    public int ContractualProductionTransferRawMaterialDetailsId { get; set; }

    public int ContractualProductionTransferRawMaterialId { get; set; }

    public int RawMaterialId { get; set; }

    public int MeasurementUnitId { get; set; }

    public decimal ConversionRatio { get; set; }

    public decimal Quantity { get; set; }

    public string ReferenceKey { get; set; }

    public virtual ContractualProductionTransferRawMaterial ContractualProductionTransferRawMaterial { get; set; }

    public virtual MeasurementUnit MeasurementUnit { get; set; }

    public virtual Product RawMaterial { get; set; }
}