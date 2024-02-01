
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

[ModelMetadataType(typeof(SalesPaymentReceiveMetadata))]
public partial class SalesPaymentReceive : VmsBaseModel
{
	[NotMapped]
	public string SalesIdReference { get; set; }
}
public class SalesPaymentReceiveMetadata
{
	[Required]
	public decimal ReceiveAmount { get; set; }

}