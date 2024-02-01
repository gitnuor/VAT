
using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

public partial class SalesDetail : VmsBaseModel
{
	[NotMapped]
	public string ReasonOfReturn { get; set; }
	[NotMapped]
	public string SalesIdReference { get; set; }
}