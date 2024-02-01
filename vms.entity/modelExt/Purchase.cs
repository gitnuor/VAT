
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

[ModelMetadataType(typeof(PurchaseMetadata))]
public partial class Purchase : VmsBaseModel
{
	[NotMapped]
	public string VDS => IsVatDeductedInSource ? "Yes" : "No";
	[NotMapped]
	public string PurchaseStatus => IsComplete ? "Posted" : "Draft";
}
public class PurchaseMetadata
{
	[Display(Name = "Expt. Del. Date")]
	public string ExpectedDeliveryDate { get; set; }
	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
	public System.DateTime VatChallanIssueDate { get; set; }
}