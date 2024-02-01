using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vms.entity.viewModels.SalesReport;

public class SalesReportByBranchParameterViewModel : SalesReportParameterViewModel
{
    public SalesReportByBranchParameterViewModel()
    {
        BranchSelectListItems = new List<CustomSelectListItem>();
    }

    [DisplayName("Branch")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} is required!")]
    public int BranchId { get; set; }

    public IEnumerable<CustomSelectListItem> BranchSelectListItems { get; set; }
}