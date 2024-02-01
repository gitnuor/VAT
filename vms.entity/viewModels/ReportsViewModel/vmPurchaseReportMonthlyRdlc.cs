using System;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmPurchaseReportMonthlyRdlc
{
    public int PurchaseId { get; set; }
    public int VendorId { get; set; }
    public string VendorName { get; set; }
    public string VendorBIN { get; set; }
    public string VendorAddress { get; set; }
    public string VendorInvoiceNo { get; set; }
    public string InvoiceNo { get; set; }
    public int OrganizationId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int PurchaseTypeId { get; set; }
    public string PurchaseTypeName { get; set; }
    public int PurchaseReasonId { get; set; }
    public string PurchaseReason { get; set; }
    public string ProductName { get; set; }
    public int PurchaseQty { get; set; }
    public string MeasurementUnitName { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? ProductPrice { get; set; }
    public decimal VatPercent { get; set; }
    public decimal? ProductVAt { get; set; }
    public decimal? ProductPriceWithVat { get; set; }
    public string OrgName { get; set; }
    public string OrgAddress { get; set; }        
    public string FromDate { get; set; }
    public string ToDate { get; set; }
}