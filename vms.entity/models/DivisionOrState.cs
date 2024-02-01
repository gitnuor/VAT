﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class DivisionOrState
{
    public int DivisionOrStateId { get; set; }

    public int OrganizationId { get; set; }

    public int CountryId { get; set; }

    public string DivisionOrStateName { get; set; }

    public string DivisionOrStateShortName { get; set; }

    public string ReferenceKey { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public virtual Country Country { get; set; }

    public virtual ICollection<DistrictOrCity> DistrictOrCities { get; set; } = new List<DistrictOrCity>();

    public virtual Organization Organization { get; set; }
}