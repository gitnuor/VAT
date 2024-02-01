using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.Customer;

public class CustomerPostDto
{

	[DisplayName("Token")][Required] public string Token { get; set; }

	[Display(Name = "Customer ID")]
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(100)]
    public string CustomerId { get; set; }

    [Display(Name = "Customer Type ID")]
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(100)]
    public string CustomerTypeId { get; set; }

    [Display(Name = "Customer Name")]
    [Required(ErrorMessage = "{0} is required")]
    [MaxLength(200)]
	public string Name { get; set; }

    [Display(Name = "Account Code")]
    [MaxLength(50)]
	public string AccountCode { get; set; }

    [Display(Name = "BIN")]
    [MaxLength(50)]
	public string Bin { get; set; }

    [Display(Name = "NID")]
    [MaxLength(50)]
	public string Nidno { get; set; }

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
    public string ContactNo { get; set; } //todo: Fix in database

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

	public string BusinessNatureId { get; set; }

	public string BusinessCategoryId { get; set; }

	public string BusinessCategoryDescription { get; set; }

	public string ContactPerson { get; set; }

	public string ContactPersonDesignation { get; set; }

	public string ContactPersonMobile { get; set; }

	public string ContactPersonEmailAddress { get; set; }

	public int? DeliveryCountryId { get; set; }

	public int? DeliveryDistrictOrCityId { get; set; }

	public int? DeliveryDivisionOrStateId { get; set; }

	public string DeliveryAddress { get; set; }

	public string DeliveryContactPerson { get; set; }

	public string DeliveryContactPersonDesignation { get; set; }

	public string DeliveryContactPersonMobile { get; set; }

	public string DeliveryContactPersonEmailAddress { get; set; }

	public string BankAccountNo { get; set; }

	public string BankRoutingCode { get; set; }

	public int? BankId { get; set; }

	public string BankBranchName { get; set; }

	public int? BankBranchCountryId { get; set; }

	public int? BankBranchDistrictOrCityId { get; set; }

	public string BankBranchAddress { get; set; }
}