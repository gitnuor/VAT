﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class OrgBranchType
{
    public int OrgBranchTypeId { get; set; }

    public string BranchTypeName { get; set; }

    public string BranchTypeNameBangla { get; set; }

    public virtual ICollection<OrgBranch> OrgBranches { get; set; } = new List<OrgBranch>();
}