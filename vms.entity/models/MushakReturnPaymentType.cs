﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class MushakReturnPaymentType
{
    public int MushakReturnPaymentTypeId { get; set; }

    public int NbrEconomicCodeId { get; set; }

    public string SubFormId { get; set; }

    public string SubFormName { get; set; }

    public string NoteNo { get; set; }

    public string TypeName { get; set; }

    public string TypeNameBn { get; set; }

    public bool IsActive { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public int PaymentReasonId { get; set; }

    public virtual ICollection<MushakReturnPayment> MushakReturnPayments { get; set; } = new List<MushakReturnPayment>();

    public virtual NbrEconomicCode NbrEconomicCode { get; set; }
}