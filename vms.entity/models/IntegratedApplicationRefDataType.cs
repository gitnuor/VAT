﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class IntegratedApplicationRefDataType
{
    public int IntegratedApplicationRefDataTypeId { get; set; }

    public string DataTypeName { get; set; }

    public string Description { get; set; }

    public virtual ICollection<IntegratedApplicationRefDatum> IntegratedApplicationRefData { get; set; } = new List<IntegratedApplicationRefDatum>();
}