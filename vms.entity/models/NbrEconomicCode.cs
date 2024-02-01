﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class NbrEconomicCode
{
    public int NbrEconomicCodeId { get; set; }

    public int NbrEconomicCodeTypeId { get; set; }

    public string EconomicTitle { get; set; }

    public string EconomicCode { get; set; }

    public string UpdatedEconomicCode { get; set; }

    public string Code1stDisit { get; set; }

    public string Code2ndDisit { get; set; }

    public string Code3rdDisit { get; set; }

    public string Code4thDisit { get; set; }

    public string Code5thDisit { get; set; }

    public string Code6thDisit { get; set; }

    public string Code7thDisit { get; set; }

    public string Code8thDisit { get; set; }

    public string Code9thDisit { get; set; }

    public string Code10thDisit { get; set; }

    public string Code11thDisit { get; set; }

    public string Code12thDisit { get; set; }

    public string Code13thDisit { get; set; }

    public bool IsActive { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public virtual ICollection<MushakReturnPaymentType> MushakReturnPaymentTypes { get; set; } = new List<MushakReturnPaymentType>();

    public virtual NbrEconomicCodeType NbrEconomicCodeType { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}