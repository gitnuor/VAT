﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class OrganizationConfigurationStringType
{
    public int OrganizationConfigurationStringTypeId { get; set; }

    public int OrganizationConfigurationAreaId { get; set; }

    public string OrganizationConfigurationStringTypeName { get; set; }

    public string DefaultValue { get; set; }

    public virtual OrganizationConfigurationArea OrganizationConfigurationArea { get; set; }
}