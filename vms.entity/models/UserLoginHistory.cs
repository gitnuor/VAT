﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class UserLoginHistory
{
    public long UserLoginHistoryId { get; set; }

    public int OrganizationId { get; set; }

    public int UserId { get; set; }

    public bool IsLoginAttemptSuccess { get; set; }

    public string ReasonOfFail { get; set; }

    public DateTime LoginTime { get; set; }

    public string UserClientDetailInformation { get; set; }

    public string UserCookie { get; set; }

    public virtual Organization Organization { get; set; }

    public virtual User User { get; set; }
}