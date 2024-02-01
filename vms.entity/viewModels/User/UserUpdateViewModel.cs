using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.User;

public class UserUpdateViewModel
{
    public int UserId { get; set; }
    public string EncryptedId { get; set; }

    [Required(ErrorMessage = "{0} is required!")]
    [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; }

    [RegularExpression("^[A-Z0-9a-z._%+-]+@[a-zA-Z_-]+?.[a-zA-Z]{2,3}$", ErrorMessage = "Entered Email Address is not valid.")]
    [Display(Name = "Username (Email Address)")]
    [MaxLength(60, ErrorMessage = "Length should not be greater than 60 characters.")]
    [Required]
    public string EmailAddress { get; set; }

    //[Display(Name = "UserType")]
    //[Required]
    //public int UserTypeId { get; set; }

    //[Display(Name = "Role")]
    ////[Required]
    //public int RoleId { get; set; }

    //[Display(Name = "Expiry Date")]
    //[DataType(DataType.DateTime)]
    //public DateTime? ExpiryDate { get; set; }

    [Required]
    [Display(Name = "Company Representative")]
    public bool IsCompanyRepresentative { get; set; }

    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    [StringLength(11, ErrorMessage = "Mobile cannot be longer than 11 characters.")]
    [Required]
    public string Mobile { get; set; }
    [Required]
    [Display(Name = "NID")]
    [MinLength(10)]
    public string NidNo { get; set; }
    [Display(Name = "TIN")]
    public string TinNo { get; set; }
    [Required]
    public string Address { get; set; }
    //public IEnumerable<CustomSelectListItem> Roles;
    //public IEnumerable<CustomSelectListItem> UserTypes;
}