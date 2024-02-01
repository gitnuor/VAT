using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmMonthlyPurchaseReport
{        
    //[Required(ErrorMessage = "Month is Required")]
    public int Month { get; set; }
    //[Required(ErrorMessage = "Year is Required")]
    //[Range(minimum: 2010, maximum: 2050, ErrorMessage = "Year must be Between 2010 and 2050")]
    public int Year { get; set; }
	[DisplayName("From Date")]
	[DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
	[DateLessThan(nameof(To), AllowEquality = true,
		ErrorMessage = "From Date must be less than or equal to To Date.")]
	public DateTime? From { get; set; }
	[DisplayName("To Date")]
	//[DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
	public DateTime? To { get; set; }
    public int Reason { get; set; }
    public string OrgName { get; set; }
    public string OrgAddress { get; set; }
    public List<SpMonthlyPurchaseReport> PurchaseList { get; set; }
    [DisplayName("Report Option")]
    [Range(1, 4, ErrorMessage = "Please select an option")]
    public int ReportProcessOptionId { get; set; }

    public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
}