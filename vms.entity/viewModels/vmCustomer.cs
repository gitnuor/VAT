using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmCustomer
{
    public vmCustomer()
    {
        Countries = new List<CustomSelectListItem>();
        DivisionList = new List<CustomSelectListItem>();
        DistrictList = new List<CustomSelectListItem>();
        CustomsAndVatCommissionarates = new List<CustomSelectListItem>();
        BusinessNatureList = new List<CustomSelectListItem>();
        BusinessCategoryList = new List<CustomSelectListItem>();
        BankList = new List<CustomSelectListItem>();
        DeliveryDivisionList = new List<CustomSelectListItem>();
        DeliveryDistrictList = new List<CustomSelectListItem>();
        BankBranchDistrictList = new List<SelectListItem>();
    }
    public string EncryptedId { get; set; }
    //public int CustomerId { get; set; }
    [DisplayName("Customer Name")]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
    [Required]
    public string Name { get; set; }

    [DisplayName("Organization")]
    public int? OrganizationId { get; set; }
    [DisplayName("Customer Organization")]
    public int? CustomerOrganizationId { get; set; }

    [DisplayName("Contact No.")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    [Required]
    public string PhoneNo { get; set; }
    [DisplayName("Delivery Contact Person Mobile")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string DeliveryContactPersonMobile { get; set; }

    [DisplayName("Email Address")]
    [StringLength(60, ErrorMessage = "Email can not be more than 60 characters")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string EmailAddress { get; set; }
    [DisplayName("Customs Vat commissionerate")]
    public int? CustomsAndVatcommissionarateId { get; set; }
    [DisplayName("Address")]
    [StringLength(150, ErrorMessage = "Address can not be more than 150 characters")]
    [Required]
    public string Address { get; set; }
    [DisplayName("BIN")]
    [StringLength(18, ErrorMessage = "BIN can not be more than 18 characters")]
    // [Required]
    public string Bin { get; set; }
    [Display(Name = "NID No.")]
    [StringLength(20, ErrorMessage = "NID No. can not be more than 20 characters")]
    // [Required]
    public string Nidno { get; set; }
    // public bool IsRequireBranch { get; set; }
	[DisplayName("Reference Key")]
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
    [Display(Name = "Credit Limit")]
    public decimal? CreditLimit { get; set; }

	[Display(Name = "Credit Period in Days")]
    public int? CreditPeriodInDay { get; set; }
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
    public virtual Bank Bank { get; set; }
    public virtual Country BankBranchCountry { get; set; }
    public virtual DistrictOrCity BankBranchDistrictOrCity { get; set; }
    public virtual BusinessCategory BusinessCategory { get; set; }
    public virtual BusinessNature BusinessNature { get; set; }
    public virtual Country Country { get; set; }
    public virtual CustomsAndVatcommissionarate CustomsAndVatcommissionarate { get; set; }
    public virtual Organization Organization { get; set; }

    public IEnumerable<CustomSelectListItem> BankList;
    public IEnumerable<CustomSelectListItem> Countries;
    public IEnumerable<CustomSelectListItem> DivisionList;
    public IEnumerable<CustomSelectListItem> DistrictList;
    public IEnumerable<CustomSelectListItem> DeliveryDivisionList;
    public IEnumerable<CustomSelectListItem> DeliveryDistrictList;
    public IEnumerable<SelectListItem> BankBranchDistrictList;
    public IEnumerable<CustomSelectListItem> CustomsAndVatCommissionarates { get; set; }
    public IEnumerable<CustomSelectListItem> BusinessNatureList { get; set; }
    public IEnumerable<CustomSelectListItem> BusinessCategoryList { get; set; }
    public string Status => IsActive ? "Active" : "Inactive";
}