using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmMushakHighValDisplay
{       
    public int Year { get; set; }
    public int Month { get; set; }
    public VmMushakHighValue VmHigh { get; set; }
    //public List<SelectListItem> YearList { get; set; }
    //public List<SelectListItem> MonthList { get; set; }
    public List<SelectListItem> LanguageList { get; set; }
    public string ReportUrl { get; set; }
    public int Language { get; set; }
    public int OrgId { get; set; }
    public string OrgName { get; set; }
    public string OrgAddress { get; set; }
    public string OrgBin { get; set; }
	public IEnumerable<CustomSelectListItem> YearList { get; set; }
	public IEnumerable<CustomSelectListItem> MonthList { get; set; }
}