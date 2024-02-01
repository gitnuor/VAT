
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

[ModelMetadataType(typeof(PurchasePaymentMetadata))]
public partial class PurchasePayment : VmsBaseModel
{
	[NotMapped]
	public string PurchaseIdReference { get; set; }
}
public class PurchasePaymentMetadata
{
	[Required]
	public decimal PaidAmount { get; set; }

}