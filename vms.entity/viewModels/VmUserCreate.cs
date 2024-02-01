using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class VmUserCreate
{
    public int UserId { get; set; }
    [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "Full-Name length should be between 4 and 100 characters.")]
    [Display(Name = "Full-Name")]
    [Required]
    public string FullName { get; set; }
    [RegularExpression("^[A-Z0-9a-z._%+-]+@[a-zA-Z_-]+?.[a-zA-Z]{2,3}$", ErrorMessage = "Entered Email Address is not valid.")]
    [Display(Name = "Email Address/Username")]
    [MaxLength(60, ErrorMessage = "Email Address/Username length should not be greater than 60 characters.")]
    [Required]
    public string EmailAddress { get; set; }
    public string Mobile { get; set; }
    public string NidNo { get; set; }
    public string TinNo { get; set; }
    public string Address { get; set; }
    public string UserImageUrl { get; set; }
    public string Password { get; set; }
    public int UserTypeId { get; set; }
    public int RoleId { get; set; }
    public DateTime? ExpiryDate { get; set; }
}