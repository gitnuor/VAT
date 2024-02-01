
using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

public partial class PurchaseDetail : VmsBaseModel
{
	[NotMapped]
	public string PurchaseIdReference { get; set; }
        
}