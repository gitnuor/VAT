using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vms.entity.viewModels.SalesReport;

public class SalesReportByCustomerParameterViewModel : SalesReportParameterViewModel
{
    public SalesReportByCustomerParameterViewModel()
    {
        CustomerSelectListItems = new List<SelectListItem>();
    }

    [DisplayName("Customer")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} is required!")]
    public int CustomerId { get; set; }

    public IEnumerable<SelectListItem> CustomerSelectListItems { get; set; }
}