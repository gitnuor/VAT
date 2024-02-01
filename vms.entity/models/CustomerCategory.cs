﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class CustomerCategory
{
    public int CustomerCategoryId { get; set; }

    public int OrganizationId { get; set; }

    public string CategoryName { get; set; }

    public string Remarks { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedTime { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual Organization Organization { get; set; }
}