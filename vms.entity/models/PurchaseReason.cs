﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class PurchaseReason
{
    public int PurchaseReasonId { get; set; }

    public string Reason { get; set; }

    public bool IsRebatable { get; set; }

    public bool IsForOfficeUse { get; set; }

    public bool IsForFactoryUse { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}