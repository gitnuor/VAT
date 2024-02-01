using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(UserMetadata))]
public partial class User : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;

	[NotMapped]
	public string Status => IsActive ? "Active" : "Inactive";

	[Required(ErrorMessage = "{0} is required!")]
	[Display(Name = "Image")]
	[NotMapped]
	public IFormFile UserImage { get; set; }
	[Required(ErrorMessage = "{0} is required!")]
	[Display(Name = "Signature")]
	[NotMapped]
	public IFormFile UserSignature { get; set; }

}
public class UserMetadata 
{
	[Required(ErrorMessage = "{0} is required!")]
	[StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
	[Display(Name = "Full Name")]
	public string FullName { get; set; }

	[StringLength(100, ErrorMessage = "User Name cannot be longer than 100 characters.")]
	[Required]
	[Display(Name = "User Name")]
	public string UserName { get; set; }

	[RegularExpression("^[A-Z0-9a-z._%+-]+@[a-zA-Z_-]+?.[a-zA-Z]{2,3}$", ErrorMessage = "Entered Email Address is not valid.")]
	[Display(Name = "Username (Email Address)")]
	[MaxLength(60, ErrorMessage = "Length should not be greater than 60 characters.")]
	[Required]
	public string EmailAddress { get; set; }

	[Display(Name = "UserType")]
	[Required]
	public int UserTypeId { get; set; }
       
	[Display(Name = "Role")]
	[Required]
	public int RoleId { get; set; }

	[Display(Name = "Organization")]
	public int OrganizationId { get; set; }

	[Display(Name = "Expiry Date")]
	[DataType(DataType.DateTime)]
	public DateTime ExpiryDate { get; set; }

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
}