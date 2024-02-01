﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class CustomerDeliveryAddress
{
    public int CustomerDeliveryAddressId { get; set; }

    public int? OrganizationId { get; set; }

    public int CustomerId { get; set; }

    public string DeliveryAddress { get; set; }

    public string DeliveryContactPerson { get; set; }

    public string DeliveryContactPersonDesignation { get; set; }

    public string DeliveryContactPersonMobile { get; set; }

    public string DeliveryContactPersonEmailAddress { get; set; }

    public string ReferenceKey { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public long? ApiTransactionId { get; set; }

    public long? ExcelDataUploadId { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Organization Organization { get; set; }
}