﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class OrganizationConfig
{
    public int OrganizationConfigId { get; set; }

    public int OrganizationId { get; set; }

    public int OrganizationConfigTypeId { get; set; }

    public string ConfigValue { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual OrganizationConfigType OrganizationConfigType { get; set; }
}