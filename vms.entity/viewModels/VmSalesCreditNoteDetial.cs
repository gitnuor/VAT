using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class VmSalesCreditNoteDetial
{
	//Details Field
	public int SalesDetailId { get; set; }
	[DisplayName("Product")]
	[Required(ErrorMessage = "Product is Required")]
	public int ProductId { get; set; }
	[DisplayName("Return Quantity")]
	[Required(ErrorMessage = "Return Quantity is Required")]
	public decimal ReturnQuantity { get; set; }
	[DisplayName("Return Amount")]
	public decimal ReturnAmount { get; set; }
	[DisplayName("Return U.Price")]
	public decimal ReturnUnitPrice { get; set; }
	[DisplayName("Return VAT (%)")]
	public decimal ReturnVatParcent { get; set; }

	[DisplayName("Return VAT")]
	public decimal ReturnVat { get; set; }

	[DisplayName("Return SD (%)")]
	public decimal ReturnSdParcent { get; set; }

	[DisplayName("Return SD")]
	public decimal ReturnSd { get; set; }
	public decimal Quantity { get; set; }
	public int MeasurementUnitId { get; set; }
	[DisplayName("M. Unit")]
	public string MeasurementUnitName { get; set; }
	[DisplayName("Amount")]
	public decimal Amount { get; set; }
	[DisplayName("Unit Price")]
	public decimal UnitPrice { get; set; }
	[DisplayName("VAT (%)")]
	public decimal VatParcent { get; set; }

	[DisplayName("VAT")]
	public decimal Vat { get; set; }

	[DisplayName("SD (%)")]
	public decimal SdParcent { get; set; }

	[DisplayName("SD")]
	public decimal Sd { get; set; }
	public string ReasonOfReturnInDetail { get; set; }
	//Detail Field
}