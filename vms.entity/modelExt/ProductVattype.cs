using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(ProductVattypeMetadata))]
public partial class ProductVattype : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;
}
public class ProductVattypeMetadata
{

	[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
	[Required]
	public string Name { get; set; }
	[Display(Name = "Default VAT (%)")]
	[Required]
	public decimal DefaultVatPercent { get; set; }
	[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
	[Display(Name = "Display Name")]
	[Required]
	public string DisplayName { get; set; }

	[Display(Name = "SD (%)")]
	public string SupplementaryDutyPercent { get; set; }

	[Display(Name = "Purchase Type")]
	public string PurchaseType { get; set; }

	[Display(Name = "Sales Type")]
	public string SalesType { get; set; }

	[Display(Name = "Transaction Type")]
	public string TransactionType { get; set; }

}