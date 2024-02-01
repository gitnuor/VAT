
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(PriceSetupMetadata))]
public partial class PriceSetup : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> MeasurementUnits;
	public IEnumerable<CustomSelectListItem> OverHeadCost;
	public string ProductName;
	public string MeasurementUnitName;
	public string ProductGroupName;
	public string ProductCategoryName;
	public string HSCode;
	//public int OverHeadCostId { get; set; }
}
public class PriceSetupMetadata
{

	[Required]
	[Display(Name = "Purchase Unit Price")]
	public decimal PurchaseUnitPrice { get; set; }
	[Required]
	[Display(Name = "Sales Unit Price")]
	[Range(0.0001, Double.MaxValue, ErrorMessage = "Price should be greater than 0")]
	public decimal SalesUnitPrice { get; set; }
	[Display(Name = "Base Tp")]
	public decimal? BaseTp { get; set; }
}