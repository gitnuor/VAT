﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.NewReports;

public class vmSalesVDSListReport
{
    public vmSalesVDSListReport()
    {
        saleVDSList = new List<Sale>();
    }
    public IEnumerable<Sale> saleVDSList { get; set; }
    public HeaderModel head { get; set; }
    [DisplayName("From Date")]
    [DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
    [DateLessThan(nameof(ToDate), AllowEquality = true, ErrorMessage = "From Date must be less than or equal to To Date.")]
    public DateTime FromDate { get; set; }
    [DisplayName("To Date")]
    [DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
    public DateTime ToDate { get; set; }

    [DisplayName("Report Option")]
    [Range(1, 4, ErrorMessage = "Please select an option")]
    public int ReportProcessOptionId { get; set; }
    public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
}