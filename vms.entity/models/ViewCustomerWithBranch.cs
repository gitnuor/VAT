﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ViewCustomerWithBranch
{
    public long? SlNo { get; set; }

    public int CustomerBranchId { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; }

    public string CustomerBin { get; set; }

    public string CustomerNid { get; set; }

    public string CustomerAddress { get; set; }

    public string CustomerEmailAddress { get; set; }

    public string CustomerContactNo { get; set; }

    public string ContactPerson { get; set; }

    public string ContactPersonDesignation { get; set; }

    public string ContactPersonMobile { get; set; }

    public string DeliveryAddress { get; set; }

    public int? OrganizationId { get; set; }

    public int OrgBranchId { get; set; }

    public string BranchName { get; set; }

    public string BranchAddress { get; set; }
}