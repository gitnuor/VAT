﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vms.entity.viewModels.PurchaseReport;

public class PurchaseReportByBranchAndVendorParameterViewModel : PurchaseReportParameterViewModel
{
    public PurchaseReportByBranchAndVendorParameterViewModel()
    {
        VendorSelectListItems = new List<CustomSelectListItem>();
        BranchSelectListItems = new List<CustomSelectListItem>();
    }

    [DisplayName("Vendor")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} is required!")]
    public int VendorId { get; set; }

    [DisplayName("Branch")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} is required!")]
    public int BranchId { get; set; }

    public IEnumerable<CustomSelectListItem> VendorSelectListItems { get; set; }
    public IEnumerable<CustomSelectListItem> BranchSelectListItems { get; set; }
}