using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmUserLoginHistory : VmReportBase
{
    public VmUserLoginHistory()
    {
        UserList = new List<CustomSelectListItem>();
        UserLoginHistories = new List<UserLoginHistory>();
    }
    [DisplayName("From Date")]
    [DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
    [DateLessThan(nameof(ToDate), AllowEquality = true, ErrorMessage = "From Date must be less than or equal to To Date")]
    [Required(ErrorMessage = "From Date is required")]
    public DateTime FromDate { get; set; }
    [DisplayName("To Date")]
    [DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
    [Required(ErrorMessage = "To Date is required")]
    public DateTime ToDate { get; set; }
    [DisplayName("User")]
    public int UserId { get; set; }
    public List<UserLoginHistory> UserLoginHistories { get; set; }
    public IEnumerable<CustomSelectListItem> UserList { get; set; }
}