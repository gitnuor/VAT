using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmExpenseReport
{
    [DisplayName("From Date")]
    [DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
    [DateLessThan(nameof(ToDate), AllowEquality = true, ErrorMessage = "From Date must be less than or equal to To Date")]
    [Required(ErrorMessage ="From Date is required")]
    public DateTime FromDate { get; set; }
    [DisplayName("To Date")]
    [DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
    [Required(ErrorMessage = "To Date is required")]
    public DateTime ToDate { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int Reason { get; set; }
    public string OrgName { get; set; }
    public string OrgAddress { get; set; }
    public List<SpMonthlyPurchaseReport> PurchaseList { get; set; }
}