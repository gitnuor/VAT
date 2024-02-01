using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class SpPurchaseSaleCalcBook
{
    [Key]
    public long? SlNo { get; set; }
    public int? OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationAddress { get; set; }
    public string OrganizationBin { get; set; }
    public decimal? InitialQty { get; set; }
    public int? ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductCode { get; set; }
    public string ProductHsCode { get; set; }
    public string ProductModel { get; set; }
    public int? PurchaseDetailId { get; set; }
    public decimal? PurchaseQty { get; set; }
    public decimal? PurchasePriceWithoutVat { get; set; }
    public decimal? QtyAfterPurchase { get; set; }
    public decimal? ProductPriceWithoutVatAfterPurchase { get; set; }
    public int? VendorId { get; set; }
    public string VendorName { get; set; }
    public string VendorBinOrNidNo { get; set; }
    public string VendorAddress { get; set; }
    public string PurcVatChallanOrBillOfEntryNo { get; set; }
    public DateTime? PurcVatChallanOrBillOfEntryDate { get; set; }
    public int? SalesDetailId { get; set; }
    public decimal? SoldQty { get; set; }
    public decimal? SalesPriceWithoutVat { get; set; }
    public decimal? SalesSupplimentaryDuty { get; set; }
    public decimal? SalesVat { get; set; }
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string CustomerBinOrNidNo { get; set; }
    public string SalesVatChallanNo { get; set; }
    public DateTime? SalesVatChallanDate { get; set; }
    public DateTime? TransactionTime { get; set; }
    public decimal? InitPriceWithoutVat { get; set; }
    public int? PurchaseId { get; set; }
    public int? SalesId { get; set; }
    public decimal? ClosingProdQty { get; set; }
    public decimal? ClosingTotalPriceWithoutVat { get; set; }
    public int? MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
    public string OperationType { get; set; }
    public string OperationTypeEng { get; set; }
	public string BranchName { get; set; }
}