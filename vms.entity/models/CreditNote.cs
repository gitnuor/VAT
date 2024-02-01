﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class CreditNote
{
    public int CreditNoteId { get; set; }

    public int SalesId { get; set; }

    public string VoucherNo { get; set; }

    public string CreditNoteChallanNo { get; set; }

    public string ReasonOfReturn { get; set; }

    public DateTime ReturnDate { get; set; }

    public int? MushakGenerationId { get; set; }

    public string ReferenceKey { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public long? ApiTransactionId { get; set; }

    public int? VehicleTypeId { get; set; }

    public string VehicleName { get; set; }

    public string VehicleRegNo { get; set; }

    public string VehicleDriverName { get; set; }

    public string VehicleDriverContactNo { get; set; }

    public bool IsCreditNoteChallanPrinted { get; set; }

    public DateTime? CreditNoteChallanPrintTime { get; set; }

    public virtual ICollection<CreditNoteDetail> CreditNoteDetails { get; set; } = new List<CreditNoteDetail>();

    public virtual MushakGeneration MushakGeneration { get; set; }

    public virtual Sale Sales { get; set; }

    public virtual VehicleType VehicleType { get; set; }
}