using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class SpPurchaseCalcBook
{
    [Key]
    public long SlNo { get; set; }
    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationBin { get; set; }
    public int? PurchaseDetailId { get; set; }
    public int? PurchaseId { get; set; }
    public string InvoiceNo { get; set; }
    public string VendorInvoiceNo { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? InitPriceWithoutVat { get; set; }
    public string VatChallanOrBillOfEntry { get; set; }
    public DateTime? VatChallanOrBillOfEntryDate { get; set; }
    public string VendorName { get; set; }
    public string VendorAddress { get; set; }
    public string VendorBinOrNid { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal? PurchaseQty { get; set; }
    public decimal? PriceWithoutVat { get; set; }
    public decimal? SupplimentaryDuty { get; set; }
    public decimal? VATAmount { get; set; }
    public decimal? TotalProdQty { get; set; }
    public decimal? TotalProdPrice { get; set; }
    public decimal? PriceWithoutVatForUsedInProduction { get; set; }
    public decimal? ClosingProdQty { get; set; }
    public decimal? ClosingTotalPrice { get; set; }
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
    public DateTime OperationTime { get; set; }
    public string OperationType { get; set; }
    public string OrganizationAddress { get; set; }
    public decimal? InitialQty { get; set; }
    public decimal? UsedInProductionQty { get; set; }
	public string BranchName { get; set; }
}