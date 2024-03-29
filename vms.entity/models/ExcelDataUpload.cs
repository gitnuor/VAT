﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ExcelDataUpload
{
    public long ExcelDataUploadId { get; set; }

    public int ExcelUploadedDataTypeId { get; set; }

    public DateTime? UploadTime { get; set; }

    public string UploadedFileName { get; set; }

    public string StoredFileName { get; set; }

    public string StoredFilePath { get; set; }

    public int? OrganizationId { get; set; }

    public bool? IsProcessed { get; set; }

    public string ProcessingMessage { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual ICollection<ExcelSimplifiedLocalPurchase> ExcelSimplifiedLocalPurchases { get; set; } = new List<ExcelSimplifiedLocalPurchase>();

    public virtual ICollection<ExcelSimplifiedLocalSaleCalculateByVat> ExcelSimplifiedLocalSaleCalculateByVats { get; set; } = new List<ExcelSimplifiedLocalSaleCalculateByVat>();

    public virtual ICollection<ExcelSimplifiedPurchase> ExcelSimplifiedPurchases { get; set; } = new List<ExcelSimplifiedPurchase>();

    public virtual ICollection<ExcelSimplifiedSalse> ExcelSimplifiedSalses { get; set; } = new List<ExcelSimplifiedSalse>();

    public virtual ExcelUploadedDataType ExcelUploadedDataType { get; set; }
}