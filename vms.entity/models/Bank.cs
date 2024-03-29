﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class Bank
{
    public int BankId { get; set; }

    public int OrganizationId { get; set; }

    public string Name { get; set; }

    public string NameInBangla { get; set; }

    public string ReferenceKey { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public long? ApiTransactionId { get; set; }

    public long? ExcelDataUploadId { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<MushakReturnPayment> MushakReturnPayments { get; set; } = new List<MushakReturnPayment>();

    public virtual Organization Organization { get; set; }

    public virtual ICollection<PurchaseImportTaxPayment> PurchaseImportTaxPayments { get; set; } = new List<PurchaseImportTaxPayment>();

    public virtual ICollection<PurchasePayment> PurchasePayments { get; set; } = new List<PurchasePayment>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<SalesPaymentReceive> SalesPaymentReceives { get; set; } = new List<SalesPaymentReceive>();

    public virtual ICollection<TdsPayment> TdsPayments { get; set; } = new List<TdsPayment>();

    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
}