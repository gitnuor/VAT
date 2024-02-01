using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.Customer;

public class CustomerLocalPostDto
{
	[Display(Name = "Customer ID")]
	[Required(ErrorMessage = "{0} is required")]
	[MaxLength(100)]
	public string Id { get; set; }

	[Display(Name = "Customer Name")]
	[Required(ErrorMessage = "{0} is required")]
	[MaxLength(200)]
	public string Name { get; set; }

	[Display(Name = "Customer Code")]
	[MaxLength(50)]
	public string CustomerCode { get; set; }

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

	//public int? CustomsAndVatcommissionarateId { get; set; }

	public string ServiceVatCode { get; set; }

	public string PostCode { get; set; }

	public string EmailAddress { get; set; }

	//public int? BusinessNatureId { get; set; }

	//public int? BusinessCategoryId { get; set; }

	public string BusinessCategoryDescription { get; set; }

	public string ContactPerson { get; set; }

	public string ContactPersonDesignation { get; set; }

	public string ContactPersonMobile { get; set; }

	public string ContactPersonEmailAddress { get; set; }
    [DisplayName("User Id")]
    [MaxLength(100)]
    public string UserId { get; set; }
}