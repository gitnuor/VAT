using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.SalesPriceAdjustment;

public class SalesPriceAdjustmentCreditNoteDetailViewModel
{
	//Details Field
	public int SalesDetailId { get; set; }
	[DisplayName("Quantity to change")]
	[Required(ErrorMessage = "{0} is Required")]
	public decimal QuantityToChange { get; set; }
	[DisplayName("Change Amount")]
	public decimal ChangeAmount { get; set; }
	[DisplayName("Change per Item")]
	public decimal ChangeAmountPerItem { get; set; }
	[DisplayName("Change VAT (%)")]
	public decimal ChangeVatPercent { get; set; }

	[DisplayName("Change VAT")]
	public decimal ChangeVat { get; set; }

	[DisplayName("Change SD (%)")]
	public decimal ChangeSdPercent { get; set; }

	[DisplayName("Change SD")]
	public decimal ChangeSd { get; set; }
	public decimal Quantity { get; set; }
	public int MeasurementUnitId { get; set; }
	[DisplayName("M. Unit")]
	public string MeasurementUnitName { get; set; }
	[DisplayName("Amount")]
	public decimal Amount { get; set; }
	[DisplayName("Unit Price")]
	public decimal UnitPrice { get; set; }
	[DisplayName("VAT (%)")]
	public decimal VatPercent { get; set; }

	[DisplayName("VAT")]
	public decimal Vat { get; set; }

	[DisplayName("SD (%)")]
	public decimal SdPercent { get; set; }

	[DisplayName("SD")]
	public decimal Sd { get; set; }
	public string ReasonOfChangeInDetail { get; set; }
	//Detail Field
}