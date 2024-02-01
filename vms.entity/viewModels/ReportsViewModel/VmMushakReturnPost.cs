using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmMushakReturnPost
{
    [Range(minimum: 2021, maximum: 2050, ErrorMessage = "Value Must be Between 2021 and 2050")]
    [Required(ErrorMessage = "Year is Required")]
    public int Year { get; set; }
    [Required(ErrorMessage = "Month is Required")]
    public int Month { get; set; }
    [Required(ErrorMessage = "Language is Required")]
    public int Language { get; set; }
}