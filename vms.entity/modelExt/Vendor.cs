
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

[ModelMetadataType(typeof(VendorMetadata))]
public partial class Vendor : VmsBaseModel
{
	[NotMapped]
	public IEnumerable<SelectListItem> Countries;
	[NotMapped]
	public IEnumerable<SelectListItem> CustomsAndVatCommissionarates { get; set; }
}
public class VendorMetadata
{
	[StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
	[Required]
	public string Name { get; set; }
	[StringLength(20, ErrorMessage = "Bin No cannot be longer than 20 characters.")]
	public string BinNo { get; set; }

	[Display(Name = "Contact No")]
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
	public string ContactNo { get; set; }

	[Display(Name = "Address")]
	[Required]
	[StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
	public string Address { get; set; }
	[DisplayName("Customs Vat commissionarate")]
	public int? CustomsAndVatcommissionarateId { get; set; }
	[DisplayName("Country")]
	public int? CountryId { get; set; }
}