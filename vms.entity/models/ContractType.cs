﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ContractType
{
    public int ContractTypeId { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ContractualProduction> ContractualProductions { get; set; } = new List<ContractualProduction>();
}