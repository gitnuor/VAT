﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class MushakSubmission
{
    public int MushakSubmissionId { get; set; }

    public byte MushakSubmissionTypeId { get; set; }

    public int OrganizationId { get; set; }

    public int MushakForYear { get; set; }

    public int MushakForMonth { get; set; }

    public DateTime GenerateDate { get; set; }

    public DateTime? SubmissionDate { get; set; }

    public int VatResponsiblePersonId { get; set; }

    public bool IsWantToGetBackClosingBalance { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedTime { get; set; }

    public virtual MushakSubmissionType MushakSubmissionType { get; set; }

    public virtual Organization Organization { get; set; }
}