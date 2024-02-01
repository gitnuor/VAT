using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vms.entity.viewModels.CustomerViewModel;

public class CustomerForeignCreateViewModel
{
	public int CustomerId { get; set; }

	[DisplayName("Organization")] public int? OrganizationId { get; set; }

	#region Basic Information

	[DisplayName("Name")]
	[StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters")]
	[Required]
	public string Name { get; set; }

	[DisplayName("Customer's Country")] public int? CountryId { get; set; }

	[DisplayName("Customer's District Or City")]
	public string DistrictOrCityName { get; set; }

	[DisplayName("Customer's Division Or State")]
	public string DivisionOrStateName { get; set; }

	[DisplayName("Address")]
	[StringLength(450)]
	[Required]
	public string Address { get; set; }

	[DisplayName("Post Code")]
	[StringLength(20)]
	public string PostCode { get; set; }

	[DisplayName("Customer's Contact No")]
	[StringLength(20)]
	[Required]
	public string ContactNo { get; set; }

	[DisplayName("Email Address")]
	[StringLength(100)]
	[DataType(DataType.EmailAddress)]
	[EmailAddress]
	public string EmailAddress { get; set; }

	#endregion

	#region Contact Person

	[DisplayName("Contact Person Name")]
	[StringLength(100)]
	public string ContactPerson { get; set; }

	[DisplayName("Contact Person Designation")]
	[StringLength(100)]
	public string ContactPersonDesignation { get; set; }

	[DisplayName("Contact Person Mobile")]
	[StringLength(20)]
	public string ContactPersonMobile { get; set; }

	[DisplayName("Contact Person Email Address")]
	[StringLength(100)]
	[DataType(DataType.EmailAddress)]
	[EmailAddress]
	public string ContactPersonEmailAddress { get; set; }

	#endregion


	[DisplayName("Bank Account No.")]
	[StringLength(50)]
	public string BankAccountNo { get; set; }

	[DisplayName("Bank Routing Code")]
	[StringLength(50)]
	public string BankRoutingCode { get; set; }

	[DisplayName("Bank")] public int? BankId { get; set; }

	[DisplayName("Bank Branch Name")]
	[StringLength(100)]
	public string BankBranchName { get; set; }

	[DisplayName("Bank Branch Country")] public int? BankBranchCountryId { get; set; }

	[DisplayName("Bank Branch District Or City")]
	public string BankBranchDistrictOrCityName { get; set; }

	[DisplayName("Bank Branch Address")]
	[StringLength(200)]
	public string BankBranchAddress { get; set; }

	[DisplayName("ReferenceKey")]
	[StringLength(100)]
	public string ReferenceKey { get; set; }


	#region Collections

	public IEnumerable<CustomSelectListItem> BankList = new List<CustomSelectListItem>();
	public IEnumerable<CustomSelectListItem> Countries = new List<CustomSelectListItem>();
	public IEnumerable<SelectListItem> DocumentTypeSelectList { get; set; } = new List<SelectListItem>();

	#endregion


	#region Document

	[DisplayName("Type")]
	[Required(ErrorMessage = "DocumentType is Required")]
	public int DocumentType { get; set; }

	[NotMapped, Required(ErrorMessage = "File is Required")]
	public IFormFile FileUpload { get; set; }

	public string FilePath { get; set; }

	[DisplayName("Remarks")]
	[MaxLength(50)]
	public string DocumentRemarks { get; set; }

	#endregion
}