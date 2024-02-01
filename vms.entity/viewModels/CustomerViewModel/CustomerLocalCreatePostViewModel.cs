using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;

namespace vms.entity.viewModels.CustomerViewModel;

[AtLeastOnePropertyRequired(nameof(Bin), nameof(Nidno), ErrorMessage = "Either BIN or NID is required!")]
public class CustomerLocalCreatePostViewModel
{
	public int CustomerId { get; set; }

	[DisplayName("Organization")] public int? OrganizationId { get; set; }

	#region Basic Information

	[DisplayName("Customer Name")]
	[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
	[Required]
	public string Name { get; set; }

	[Display(Name = "NID No.")]
	[StringLength(20, ErrorMessage = "NID No. can not be more than 20 characters")]
	public string Nidno { get; set; }

	[DisplayName("BIN")]
	[StringLength(18, ErrorMessage = "BIN can not be more than 18 characters")]
	public string Bin { get; set; }

	[DisplayName("TIN No.")]
	[StringLength(50)]
	public string Tinno { get; set; }

	[DisplayName("Mobile No.")]
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
	[Required]
	public string PhoneNo { get; set; }

	[DisplayName("Email Address")]
	[StringLength(60, ErrorMessage = "Email can not be more than 60 characters")]
	[DataType(DataType.EmailAddress)]
	[EmailAddress]
	public string EmailAddress { get; set; }

	[DisplayName("District")] public string DistrictOrCityName { get; set; }
	[DisplayName("Division")] public string DivisionOrStateName { get; set; }

	[DisplayName("Address")]
	[StringLength(150, ErrorMessage = "Address can not be more than 150 characters")]
	[Required]
	public string Address { get; set; }

	[DisplayName("Post Code")]
	[StringLength(20)]
	public string PostCode { get; set; }

	[DisplayName("Is Require Branch?")] public bool IsRequireBranch { get; set; }

	#endregion

	#region Delivery Information

	[DisplayName("Delivery District")] public string DeliveryDistrictOrCityName { get; set; }

	[DisplayName("Delivery Division")] public string DeliveryDivisionOrStateName { get; set; }

	[DisplayName("Delivery Address")]
	[StringLength(450)]
	public string DeliveryAddress { get; set; }

	[DisplayName("Delivery Contact Person Name")]
	[StringLength(100)]
	public string DeliveryContactPerson { get; set; }

	[DisplayName("Delivery Contact Person Designation")]
	[StringLength(100)]
	public string DeliveryContactPersonDesignation { get; set; }

	[DisplayName("Delivery Contact Person Email Address")]
	[StringLength(100)]
	[DataType(DataType.EmailAddress)]
	[EmailAddress]
	public string DeliveryContactPersonEmailAddress { get; set; }

	[DisplayName("Delivery Contact Person Mobile")]
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
	public string DeliveryContactPersonMobile { get; set; }

	#endregion

	#region Miscellaneous Infomration

	[DisplayName("Reference Key")]
	[StringLength(70, ErrorMessage = "Reference Key can not be more than 70 characters")]
	public string ReferenceKey { get; set; }

	[DisplayName("Customs, Excise and VAT Commissionerate")]
	public int? CustomsAndVatcommissionarateId { get; set; }

	[DisplayName("Service VAT Code")]
	[StringLength(50)]
	public string ServiceVatCode { get; set; }

	[DisplayName("Business Nature")] public int? BusinessNatureId { get; set; }
	[DisplayName("Business Category")] public int? BusinessCategoryId { get; set; }

	[DisplayName("Business Category Description")]
	[StringLength(450)]
	public string BusinessCategoryDescription { get; set; }

	[Display(Name = "Credit Limit")] public decimal? CreditLimit { get; set; }

	[Display(Name = "Credit Period in Days")]
	public int? CreditPeriodInDay { get; set; }

	[DisplayName("Is Registered Under Act NinetyFour?")]
	public bool IsRegisteredUnderActNinetyFour { get; set; }

	[DisplayName("Registration Number Under Act NinetyFour")]
	[StringLength(50)]
	public string RegistrationNumberUnderActNinetyFour { get; set; }


	#region TDS, VDS etc information

	[DisplayName("Is VDS?")] public bool IsVds { get; set; }
	[DisplayName("VDS Rate")] public decimal? Vdsrate { get; set; }
	[DisplayName("Is TDS?")] public bool IsTds { get; set; }
	[DisplayName("TDS Rate")] public decimal? Tdsrate { get; set; }

	#endregion

	#endregion

	#region Contact Person

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

	#endregion

	#region Banking Information

	[DisplayName("Bank Account No.")]
	[StringLength(50)]
	public string BankAccountNo { get; set; }

	[DisplayName("Bank Routing Code")]
	[StringLength(45)]
	public string BankRoutingCode { get; set; }

	[DisplayName("Bank")] public int? BankId { get; set; }

	[DisplayName("Bank Branch Name")]
	[StringLength(90)]
	public string BankBranchName { get; set; }

	[DisplayName("Bank Branch District")] public string BankBranchDistrictOrCityName { get; set; }

	[DisplayName("Bank Branch Address")]
	[StringLength(180)]
	public string BankBranchAddress { get; set; }

	#endregion

	#region Collections

	public IEnumerable<CustomerBranchCreateViewModel> CustomerBranchList { get; set; } =
		new List<CustomerBranchCreateViewModel>();

	public IEnumerable<VmsDocumentPostViewModel> Documents { get; set; } = new List<VmsDocumentPostViewModel>();

	#endregion
}