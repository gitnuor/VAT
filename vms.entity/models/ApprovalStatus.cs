﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ApprovalStatus
{
    public int ApprovalStatusId { get; set; }

    public string StatusName { get; set; }

    public bool IsApproved { get; set; }

    public bool IsRejected { get; set; }

    public bool IsComplete { get; set; }
}