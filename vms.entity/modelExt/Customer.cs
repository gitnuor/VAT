using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(CustomerMetadata))]
public partial class Customer : VmsBaseModel
{

	[NotMapped]
	public IEnumerable<CustomSelectListItem> Countries;
	[NotMapped]
	public IEnumerable<CustomSelectListItem> CustomsAndVatCommissionarates { get; set; }
        
	[NotMapped]
	public string Status => IsActive ? "Active" : "Inactive";
}
public class CustomerMetadata
{
	[Display(Name = "Name")]
	[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
	[Required]
	public string Name { get; set; }

	[Display(Name = "Organization")]
	public int OrganizationId { get; set; }

	[Display(Name = "Phone No.")]
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
	public string PhoneNo { get; set; }
	[DisplayName("Delivery Contact Person Mobile")]
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
	public string DeliveryContactPersonMobile { get; set; }

	[Display(Name = "Email Address")]
	[StringLength(60, ErrorMessage = "Email can not be more than 60 characters")]
	[Required(ErrorMessage = "Email is required")]
	[DataType(DataType.EmailAddress)]
	[EmailAddress]
	public string EmailAddress { get; set; }
	[DisplayName("Customs Vat commissionarate")]
	public int? CustomsAndVatcommissionarateId { get; set; }
	[Display(Name = "Address")]
	[StringLength(150, ErrorMessage = "Address can not be more than 150 characters")]
	public string Address { get; set; }
	[Display(Name = "BIN")]
	[StringLength(18, ErrorMessage = "BIN can not be more than 18 characters")]
	public string Bin { get; set; }
	[Display(Name = "NID No.")]
	[StringLength(20, ErrorMessage = "NID No. can not be more than 20 characters")]
	public string Nidno { get; set; }
	[Display(Name = "Reference Key")]
	[StringLength(70, ErrorMessage = "Reference Key can not be more than 70 characters")]
	public string ReferenceKey { get; set; }
	[DisplayName("Country")]
	public int? CountryId { get; set; }
	[DisplayName("TIN No.")]
	[StringLength(50)]
	public string Tinno { get; set; }
	[DisplayName("Is VDS?")]
	public bool IsVds { get; set; }
	[DisplayName("VDS Rate")]
	public decimal? Vdsrate { get; set; }
	[DisplayName("Is TDS?")]
	public bool IsTds { get; set; }
	[DisplayName("TDS Rate")]
	public decimal? Tdsrate { get; set; }
	[DisplayName("Is Registered Under Act NinetyFour?")]
	public bool IsRegisteredUnderActNinetyFour { get; set; }
	[DisplayName("Registration Number Under Act NinetyFour")]
	[StringLength(50)]
	public string RegistrationNumberUnderActNinetyFour { get; set; }
	[DisplayName("Service VAT Code")]
	[StringLength(50)]
	public string ServiceVatCode { get; set; }
	[DisplayName("District Or City")]
	public int? DistrictOrCityId { get; set; }
	[DisplayName("Division Or State")]
	public int? DivisionOrStateId { get; set; }
	[DisplayName("Post Code")]
	[StringLength(20)]
	public string PostCode { get; set; }
	[DisplayName("Is Active")]
	public bool IsActive { get; set; }
	[DisplayName("Business Nature")]
	public int? BusinessNatureId { get; set; }
	[DisplayName("Business Category")]
	public int? BusinessCategoryId { get; set; }
	[DisplayName("Business Category Description")]
	[StringLength(450)]
	public string BusinessCategoryDescription { get; set; }
	[DisplayName("Is Foreign Customer?")]
	public bool IsForeignCustomer { get; set; }
	[DisplayName("Is Full Export Oriented?")]
	public bool IsFullExportOriented { get; set; }
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
	[DisplayName("Delivery Country")]
	public int? DeliveryCountryId { get; set; }
	[DisplayName("Delivery District Or City")]
	public int? DeliveryDistrictOrCityId { get; set; }
	[DisplayName("Delivery Division Or State")]
	public int? DeliveryDivisionOrStateId { get; set; }
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
	[DisplayName("Bank Account No.")]
	[StringLength(50)]
	public string BankAccountNo { get; set; }
	[DisplayName("Bank Routing Code")]
	[StringLength(45)]
	public string BankRoutingCode { get; set; }
	[DisplayName("Bank")]
	public int? BankId { get; set; }
	[DisplayName("Bank Branch Name")]
	[StringLength(90)]
	public string BankBranchName { get; set; }
	[DisplayName("Bank Branch Country")]
	public int? BankBranchCountryId { get; set; }
	[DisplayName("Bank Branch District")]
	public int? BankBranchDistrictOrCityId { get; set; }
	[DisplayName("Bank Branch Address")]
	[StringLength(180)]
	public string BankBranchAddress { get; set; }

}