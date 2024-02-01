
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(MushakGenerationMetadata))]
public partial class MushakGeneration : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;
}
public class MushakGenerationMetadata
{


	[Display(Name = "Year")]
	[Required]
	public int MushakForYear { get; set; }

	[Display(Name = "Month")]
	[Required]
	public int MushakForMonth { get; set; }

	[Display(Name = "Generate Date")]
	[Required]
	[DisplayFormat(DataFormatString = "{0:d}")]
	public DateTime GenerateDate { get; set; }

	[DisplayFormat(DataFormatString = "{0:d}")]
	[Display(Name = "SubmissionDate")]
	public DateTime? SubmissionDate { get; set; }

	[Display(Name = "Is Submitted")]
	public int IsSubmitted { get; set; }

	[Display(Name = "Paid For Vat")]
	public int PaidAmountForVat { get; set; }

	[Display(Name = "Paid For Supp. Duty")]
	public int PaidAmountForSuppDuty { get; set; }

	[Display(Name = "Interest For Due Vat")]
	public int InterestForDueVat { get; set; }

	[Display(Name = "Interest For Due Supp. Duty")]
	public int InterestForDueSuppDuty { get; set; }


}