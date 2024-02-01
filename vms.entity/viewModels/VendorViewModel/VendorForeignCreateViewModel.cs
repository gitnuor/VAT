using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vms.entity.viewModels.VendorViewModel;

public class VendorForeignCreateViewModel
{
	public VendorForeignCreateViewModel()
	{
		Countries = new List<CustomSelectListItem>();
		BankList = new List<CustomSelectListItem>();
	}

	public string EncryptedId { get; set; }

	public int VendorId { get; set; }
	public int? OrganizationId { get; set; }

	[DisplayName("Name")]
	[StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters")]
	[Required]
	public string Name { get; set; }

	[DisplayName("Vendor's Country")] public int? CountryId { get; set; }

	[DisplayName("Vendor's District Or City")]
	public string DistrictOrCityName { get; set; }

	[DisplayName("Vendor's Division Or State")]
	public string DivisionOrStateName { get; set; }

	[DisplayName("Address")]
	[StringLength(450)]
	[Required]
	public string Address { get; set; }

	[DisplayName("Post Code")]
	[StringLength(20)]
	public string PostCode { get; set; }

	[DisplayName("Vendor's Contact No")]
	[StringLength(20)]
	[Required]
	public string ContactNo { get; set; }

	[DisplayName("Email Address")]
	[StringLength(100)]
	[DataType(DataType.EmailAddress)]
	[EmailAddress]
	public string EmailAddress { get; set; }

	[DisplayName("Is Active")] public bool IsActive { get; set; } = true;

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


	public IEnumerable<CustomSelectListItem> BankList;
	public IEnumerable<CustomSelectListItem> Countries;
}