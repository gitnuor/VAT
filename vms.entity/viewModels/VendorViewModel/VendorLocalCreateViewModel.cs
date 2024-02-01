using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vms.entity.viewModels.VendorViewModel;

public class VendorLocalCreateViewModel
{
	public VendorLocalCreateViewModel()
	{
		CustomsAndVatCommissionarates = new List<CustomSelectListItem>();
		BusinessNatureList = new List<CustomSelectListItem>();
		BusinessCategoryList = new List<CustomSelectListItem>();
		BankList = new List<CustomSelectListItem>();
	}

	public string EncryptedId { get; set; }
	public int VendorId { get; set; }
	public int OrganizationId { get; set; }

	[DisplayName("Name")]
	[StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters")]
	[Required]
	public string Name { get; set; }

	[DisplayName("BIN No.")]
	[StringLength(20)]
	public string BinNo { get; set; }

	[DisplayName("National Id No.")]
	[StringLength(50)]
	public string NationalIdNo { get; set; }

	[DisplayName("TIN No.")]
	[StringLength(50)]
	public string Tinno { get; set; }

	[DisplayName("Is VDS?")] public bool IsVds { get; set; }
	[DisplayName("VDS Rate")] public decimal? Vdsrate { get; set; }
	[DisplayName("Is TDS?")] public bool IsTds { get; set; }
	[DisplayName("TDS Rate")] public decimal? Tdsrate { get; set; }

	[DisplayName("Is Registered Under Act NinetyFour")]
	public bool IsRegisteredUnderActNinetyFour { get; set; }

	[DisplayName("Registration Number Under Act NinetyFour")]
	[StringLength(50)]
	public string RegistrationNumberUnderActNinetyFour { get; set; }

	[DisplayName("Customs And Vat Commissionarate")]
	public int? CustomsAndVatcommissionarateId { get; set; }

	[DisplayName("Service VAT Code")]
	[StringLength(50)]
	public string ServiceVatCode { get; set; }

	[DisplayName("Vendor's Country")] public int? CountryId { get; set; }

	[DisplayName("Vendor's District Or City")]
	public int? DistrictOrCityName { get; set; }

	[DisplayName("Vendor's Division Or State")]
	public int? DivisionOrStateName { get; set; }

	[DisplayName("Address")]
	[StringLength(450)]
	[Required]
	public string Address { get; set; }

	[DisplayName("Post Code")]
	[StringLength(20)]
	public string PostCode { get; set; }

	[DisplayName("Vendor's Contact No")]
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
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
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
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
	public int? BankBranchDistrictOrCityId { get; set; }

	[DisplayName("Bank Branch Address")]
	[StringLength(200)]
	public string BankBranchAddress { get; set; }

	[DisplayName("Business Nature")] public int? BusinessNatureId { get; set; }
	[DisplayName("Business Category")] public int? BusinessCategoryId { get; set; }

	[DisplayName("Business Category Description")]
	[StringLength(450)]
	public string BusinessCategoryDescription { get; set; }

	[DisplayName("Is Registered As Turn Over Organization?")]
	public bool IsRegisteredAsTurnOverOrg { get; set; }

	[DisplayName("Is Registered?")] public bool IsRegistered { get; set; }
	[DisplayName("Is Foreign Vendor?")] public bool IsForeignVendor { get; set; }

	[DisplayName("ReferenceKey")]
	[StringLength(100)]
	public string ReferenceKey { get; set; }

	public int? CreatedBy { get; set; }
	public DateTime? CreatedTime { get; set; }
	public int? ModifiedBy { get; set; }
	public DateTime? ModifiedTime { get; set; }
	public long? ApiTransactionId { get; set; }

	public IEnumerable<CustomSelectListItem> BankList;
	public IEnumerable<CustomSelectListItem> CustomsAndVatCommissionarates { get; set; }
	public IEnumerable<CustomSelectListItem> BusinessNatureList { get; set; }
	public IEnumerable<CustomSelectListItem> BusinessCategoryList { get; set; }
	public string Status => IsActive ? "Active" : "Inactive";
}