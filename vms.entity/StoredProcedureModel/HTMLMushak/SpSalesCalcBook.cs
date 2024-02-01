using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class SpSalesCalcBook
{
    [Key]
    public long SlNo { get; set; }
    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationBin { get; set; }
    public int? SalesId { get; set; }
    public int? SalesDetailId { get; set; }
    public DateTime? SalesDate { get; set; }
    public decimal? InitialQty { get; set; }
    public decimal? InitPriceWithoutVat { get; set; }
    public decimal? ProductionQty { get; set; }
    public decimal? PriceOfProdFromProduction { get; set; }
    public decimal? TotalProductionQty { get; set; }
    public decimal? TotalProductionPrice { get; set; }
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? TaxInvoicePrintedTime { get; set; }
    public int? ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal? SalesQty { get; set; }
    public decimal? TaxablePrice { get; set; }
    public decimal? ProdVatAmount { get; set; }
    public decimal? ClosingProdQty { get; set; }
    public decimal? ClosingProdPrice { get; set; }
    public int? MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }
    public string OrganizationAddress { get; set; }
    public decimal? SupplementaryDuty { get; set; }
    public DateTime OperationTime { get; set; }
    public string OperationType { get; set; }
    public string CustomerBinOrNid { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? SalesUnitPrice { get; set; }
	public string BranchName { get; set; }
}