using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.Vendor;

public class VendorLocalPostDto
{
    [Display(Name = "Supplier/Vendor ID")]
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(100)]
    public string Id { get; set; }

	[Display(Name = "Supplier/Vendor Name")]
	[Required(ErrorMessage = "{0} is required")]
	[MaxLength(200)]
	public string Name { get; set; }

	[Display(Name = "Account Code")]
	[MaxLength(50)]
	public string AccountCode { get; set; }

	[Display(Name = "Vendor Code")]
	[MaxLength(50)]
	public string VendorCode { get; set; }

	[Display(Name = "BIN")]
	[MaxLength(50)]
	public string BinNo { get; set; }

	[Display(Name = "NID")]
	[MaxLength(50)]
	public string NationalIdNo { get; set; }

	[Display(Name = "TIN")]
	[MaxLength(50)]
	public string Tinno { get; set; }

	[Display(Name = "Address")]
	[Required(ErrorMessage = "{0} is required")]
	[MaxLength(500)]
	public string Address { get; set; }

	[Display(Name = "Contact No")]
	[Required(ErrorMessage = "{0} is required")]
	[MaxLength(20)]
	public string ContactNo { get; set; }

    [Display(Name = "Is VDS?")]
	public bool? IsVds { get; set; }

    [Display(Name = "VDS Rate")]
	public decimal? Vdsrate { get; set; }

	[Display(Name = "Is TDS?")]
    public bool? IsTds { get; set; }

    [Display(Name = "TDS Rate")]
	public decimal? Tdsrate { get; set; }

	public bool? IsRegisteredUnderActNinetyFour { get; set; }

	public string RegistrationNumberUnderActNinetyFour { get; set; }

	public string CustomsAndVatcommissionarateId { get; set; }

	public string ServiceVatCode { get; set; }

	public string CountryId { get; set; }

	public string DistrictOrCityId { get; set; }

	public string DivisionOrStateId { get; set; }

	public string PostCode { get; set; }

	public string EmailAddress { get; set; }

	public string ContactPerson { get; set; }

	public string ContactPersonDesignation { get; set; }

	public string ContactPersonMobile { get; set; }

	public string ContactPersonEmailAddress { get; set; }

	public string BankAccountNo { get; set; }

	public string BankRoutingCode { get; set; }

	public string BankId { get; set; }

	public string BankBranchName { get; set; }

	public string BankBranchCountryId { get; set; }

	public string BankBranchDistrictOrCityId { get; set; }

	public string BankBranchAddress { get; set; }

	public string BusinessNatureId { get; set; }

	public string BusinessCategoryId { get; set; }

	public string BusinessCategoryDescription { get; set; }

	public bool? IsRegisteredAsTurnOverOrg { get; set; }

	public bool? IsRegistered { get; set; }

	public string UserId { get; set; }
}