using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class vmMushakUpload
{
    [Required(ErrorMessage ="Year is required")]
    [DisplayName("Year")]
    public int Year { get; set; }
    [Required(ErrorMessage = "Month is required")]
    [DisplayName("Month")]
    public string Month { get; set; }
    [Required(ErrorMessage = "File is required")]
    [DisplayName("Mushak File")]
    public IFormFile File { get; set; }
    public string Path { get; set; }
}