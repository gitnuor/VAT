using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmMushakReturnDisplay
{
    [Range(minimum: 2000, maximum: 2050, ErrorMessage = "Value Must be Between 2021 and 2050")]
    [Required(ErrorMessage ="Year is Required")]
    public int Year { get; set; }
    [Required(ErrorMessage = "Month is Required")]
    public int Month { get; set; }
    [Required(ErrorMessage = "Language is Required")]
    public int Language { get; set; }
    public VmMushakReturn VmMushakReturn { get; set; }
    public List<SelectListItem> YearList { get; set; }
    public List<SelectListItem> MonthList { get; set; }
    public List<SelectListItem> LanguageList { get; set; }
}