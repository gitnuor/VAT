using System;

namespace vms.entity.viewModels.VmPurchaseCombineParamsModels;

public class PurchaseDetailCombinedParamModel
{
	public int PurchaseDetailId { get; set; }
	public int PurchaseId { get; set; }
	public int ProductId { get; set; }
	public string ProductDescription { get; set; }
	public string HSCode { get; set; }
	public string PartNo { get; set; }
	public string SKUNo { get; set; }
	public string SKUId { get; set; }
	public string GoodsId { get; set; }
	public int ProductVattypeId { get; set; }
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
	public int MeasurementUnitId { get; set; }
	public string ReferenceKey { get; set; }
	public int? CreatedBy { get; set; }
	public DateTime? CreatedTime { get; set; }
}