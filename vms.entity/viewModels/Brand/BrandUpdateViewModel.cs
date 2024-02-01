using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.Brand;

public class BrandUpdateViewModel
{
    public int BrandId { get; set; }
    [Display(Name = "Name")]
    [Required(ErrorMessage = "{0} is required!")]
    [MaxLength(250, ErrorMessage = "Max. length of {0} should be less than {1}.")]
    public string Name { get; set; }
    [Display(Name = "Name in Bangla")]
    [MaxLength(250, ErrorMessage = "Max. length of {0} should be less than {1}.")]
    public string NameInBangla { get; set; }
    [MaxLength(500, ErrorMessage = "Max. length of {0} should be less than {1}.")]
    public string Description { get; set; }
    [Display(Name = "Reference Key")]
    [MaxLength(100, ErrorMessage = "Max. length of {0} should be less than {1}.")]
    public string ReferenceKey { get; set; }
}