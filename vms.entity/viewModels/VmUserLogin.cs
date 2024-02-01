using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class VmUserLogin
{
    [Required(ErrorMessage = "{0} is required!")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email address!")]
    [EmailAddress(ErrorMessage = "Please enter valid email address!")]
    [Display(Name = "Username")]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}