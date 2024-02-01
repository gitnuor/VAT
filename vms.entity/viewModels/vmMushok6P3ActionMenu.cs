using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class vmMushok6P3ActionMenu
{
    public int SalesId { get; set; }
    [Required(ErrorMessage ="Language is required")]
    public int Language { get; set; }
}