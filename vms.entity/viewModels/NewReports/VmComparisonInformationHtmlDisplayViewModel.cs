using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels.NewReports;

public class VmComparisonInformationHtmlDisplayViewModel
{
    public VmComparisonInformationHtmlDisplayViewModel()
    {
        YearlyComparisons = new List<SpGetYearlyComparison>();
    }
    public IEnumerable<SpGetYearlyComparison> YearlyComparisons { get; set; }
    public HeaderModel HeaderModel { get; set; }
    [DisplayName("From Year")]
    public int FromYear { get; set; }

    [DisplayName("To Year")]
    public int ToYear { get; set; }

    [DisplayName("Report Option")]
    [Range(1, 4, ErrorMessage = "Please select an option")]
    public int ReportProcessOptionId { get; set; }
    public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
}