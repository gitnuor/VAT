﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string Name { get; set; }

    public bool IsBankingChannel { get; set; }

    public bool IsMobileTransaction { get; set; }

    public bool IsCash { get; set; }

    public bool IsCheque { get; set; }

    public bool IsUseInChallanPayment { get; set; }

    public virtual ICollection<PurchasePayment> PurchasePayments { get; set; } = new List<PurchasePayment>();

    public virtual ICollection<SalesPaymentReceive> SalesPaymentReceives { get; set; } = new List<SalesPaymentReceive>();
}