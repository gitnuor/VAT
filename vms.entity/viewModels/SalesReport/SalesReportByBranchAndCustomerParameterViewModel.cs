using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vms.entity.viewModels.SalesReport;

public class SalesReportByBranchAndCustomerParameterViewModel : SalesReportParameterViewModel
{
    public SalesReportByBranchAndCustomerParameterViewModel()
    {
        CustomerSelectListItems = new List<SelectListItem>();
        BranchSelectListItems = new List<CustomSelectListItem>();
    }

    [DisplayName("Customer")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} is required!")]
    public int CustomerId { get; set; }

    [DisplayName("Branch")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} is required!")]
    public int BranchId { get; set; }

    public IEnumerable<SelectListItem> CustomerSelectListItems { get; set; }
    public IEnumerable<CustomSelectListItem> BranchSelectListItems { get; set; }
}