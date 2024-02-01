using System;

namespace vms.entity.viewModels.VmPurchaseCombineParamsModels;

public class PurchaseDetailFromApiCombinedParamModel
{
	public string PurchaseDetailId { get; set; }
	public string ProductId { get; set; }
	public string ProductDescription { get; set; }
	public string HSCode { get; set; }
	public string ProductCode { get; set; }
	public string PartNo { get; set; }
	public string SKUNo { get; set; }
	public string SKUId { get; set; }
	public string GoodsId { get; set; }
	public int ProductVatTypeId { get; set; }
	public decimal Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal DiscountPerItem { get; set; }
	public decimal CustomDutyPercent { get; set; }
	public decimal ImportDutyPercent { get; set; }
	public decimal RegulatoryDutyPercent { get; set; }
	public decimal SupplementaryDutyPercent { get; set; }
	public decimal VatPercent { get; set; }
	public decimal AdvanceTaxPercent { get; set; }
	public decimal AdvanceIncomeTaxPercent { get; set; }
	public string MeasurementUnitName { get; set; }
}