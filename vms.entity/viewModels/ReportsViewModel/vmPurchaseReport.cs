using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using vms.entity.CustomDataAnnotations;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmPurchaseReport : ReoportOption
{
    public VmPurchaseReport()
    {
        VendorList = new List<SelectListItem>();
    }

    public int PurchaseId { get; set; }
    public string InvoiceNo { get; set; }
    public int vendorId { get; set; }
    public int reason { get; set; }
    public int organizationId { get; set; }
    [DisplayName("From Date")]
    [DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
    [DateLessThan(nameof(ToDate), AllowEquality = true, ErrorMessage = "From Date must be less than or equal to To Date.")]
    public DateTime FromDate { get; set; }
    [DisplayName("To Date")]
    [DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
    public DateTime ToDate { get; set; }
    public IEnumerable<SelectListItem> VendorList { get; set; }

}