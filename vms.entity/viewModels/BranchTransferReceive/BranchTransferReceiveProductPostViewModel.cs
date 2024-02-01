using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.BranchTransferReceive;

public class BranchTransferReceiveProductPostViewModel
{
	public int BranchTransferReceiveDetailId { get; set; }
	[Required(ErrorMessage = "Product is Required")]
	[DisplayName("Product")]
	public int ProductId { get; set; }

	[DisplayName("Description")]
	[MaxLength(500)]
	public string ProductDescription { get; set; }

	[DisplayName("HS Code")]
    [MaxLength(50)]
	public string Hscode { get; set; }

    [DisplayName("Product Code")]
    [MaxLength(50)]
    public string ProductCode { get; set; }

    [DisplayName("SKU")]
	[MaxLength(50)]
	public string SKUNo { get; set; }

	[DisplayName("SKU ID")]
	[MaxLength(50)]
	public string SKUId { get; set; }

	[DisplayName("GOODS ID")]
	[MaxLength(50)]
	public string GoodsId { get; set; }
	public long? ProductTransactionBookId { get; set; }
	[DisplayName("Quantity")]
	[Required(ErrorMessage = "Quantity is Required")]
	[Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
	public decimal Quantity { get; set; }
	[Required(ErrorMessage = "Current Stock is Required")]
	[DisplayName("Cur. Stock")]
	public decimal CurrentStock { get; set; }


	[DisplayName("Unit Price")]
	[Required(ErrorMessage = "{0} is Required")]
	[Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
	public decimal UnitPrice { get; set; }

	[DisplayName("Total Price")]
	[Required(ErrorMessage = "{0} is Required")]
	[Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
	public decimal TotalPrice { get; set; }

	[DisplayName("M. Unit")]
	public int MeasurementUnitId { get; set; }
	[DisplayName("M. Unit")]
	public string MeasurementUnitName { get; set; }
	[DisplayName("Remarks")]
	public string ProductRemarks { get; set; }
}