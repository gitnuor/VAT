﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class TdsPaymentForPurchase
{
    public int TdsPaymentForPurchaseId { get; set; }

    public int TdsPaymentId { get; set; }

    public int PurchaseId { get; set; }

    public decimal TdsPaidAmount { get; set; }

    public virtual Purchase Purchase { get; set; }

    public virtual TdsPayment TdsPayment { get; set; }
}