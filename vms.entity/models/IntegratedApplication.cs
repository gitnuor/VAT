﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class IntegratedApplication
{
    public int IntegratedApplicationId { get; set; }

    public int OrganizationId { get; set; }

    public string ApplicationName { get; set; }

    public string ApplicationKey { get; set; }

    public string ApplicationUrl { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public virtual ICollection<IntegratedApplicationRefDatum> IntegratedApplicationRefData { get; set; } = new List<IntegratedApplicationRefDatum>();
}