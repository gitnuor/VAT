using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class SaleApproveOrRejectViewModel{
	public int SalesId { get; set; }
	public string EncSalesId { get; set; }
	[DisplayName("Remarks")]
	[MaxLength(500)]
	[Required]
	public string Remarks { get; set; }
	public int? UserId { get; set; }
}