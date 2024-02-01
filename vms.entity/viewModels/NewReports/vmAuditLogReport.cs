using System;
using System.Collections.Generic;
using System.ComponentModel;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.NewReports;

public class vmAuditLogReport
{
    public vmAuditLogReport()
    {
        auditLogList = new List<AuditLog>();
    }
    public IEnumerable<AuditLog> auditLogList { get; set; }
    public HeaderModel head { get; set; }
    [DisplayName("From Date")]
    [DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
    [DateLessThan(nameof(ToDate), AllowEquality = true, ErrorMessage = "From Date must be less than or equal to To Date.")]
    public DateTime FromDate { get; set; }
    [DisplayName("To Date")]
    [DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
    public DateTime ToDate { get; set; }
}