﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ProductMeasurementUnit
{
    public int ProductMeasurementUnitId { get; set; }

    public int OrganizationId { get; set; }

    public int ProductId { get; set; }

    public int MeasurementUnitId { get; set; }

    public decimal ConversionRatio { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public virtual MeasurementUnit MeasurementUnit { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual Product Product { get; set; }
}