﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class Brand
{
    public int BrandId { get; set; }

    public int OrganizationId { get; set; }

    public string Name { get; set; }

    public string NameInBangla { get; set; }

    public string Description { get; set; }

    public string ReferenceKey { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public long? ApiTransactionId { get; set; }

    public long? ExcelDataUploadId { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}