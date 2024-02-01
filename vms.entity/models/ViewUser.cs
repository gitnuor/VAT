﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ViewUser
{
    public int UserId { get; set; }

    public string FullName { get; set; }

    public string UserName { get; set; }

    public string EmployeeId { get; set; }

    public string EmployeePin { get; set; }

    public string Designation { get; set; }

    public string EmailAddress { get; set; }

    public string Mobile { get; set; }

    public string NidNo { get; set; }

    public string TinNo { get; set; }

    public string Address { get; set; }

    public string UserImageUrl { get; set; }

    public string UserSignUrl { get; set; }

    public int UserTypeId { get; set; }

    public string UserTypeName { get; set; }

    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public int? OrganizationId { get; set; }

    public bool? IsRequireBranch { get; set; }

    public bool IsActive { get; set; }

    public string ActiveStatus { get; set; }

    public string InActivationReason { get; set; }

    public DateTime? InActivationTime { get; set; }

    public bool? IsLocked { get; set; }

    public DateTime? LastLockTime { get; set; }

    public string LastLockReason { get; set; }

    public DateTime? LastLoginTime { get; set; }

    public string ReferenceKey { get; set; }

    public DateTime? ExpiryDate { get; set; }
}