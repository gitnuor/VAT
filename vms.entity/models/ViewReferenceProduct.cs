﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ViewReferenceProduct
{
    public long IntegratedApplicationRefDataId { get; set; }

    public int IntegratedApplicationId { get; set; }

    public int IntegratedApplicationRefDataTypeId { get; set; }

    public string ReferenceKey { get; set; }

    public long DataKey { get; set; }

    public int ProductId { get; set; }

    public int OrganizationId { get; set; }
}