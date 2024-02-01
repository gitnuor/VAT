﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class Right
{
    public int RightId { get; set; }

    public string RightName { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int RightCategoryId { get; set; }

    public virtual RightCategory RightCategory { get; set; }

    public virtual ICollection<RoleRight> RoleRights { get; set; } = new List<RoleRight>();
}