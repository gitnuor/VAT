using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class SpSalesTaxInvoice
{
    [Key]

    public long? Sl { get; set; }
    public int SalesId { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public string EmhCode { get; set; }
    public string VatChallanNo { get; set; }
    public string WorkOrderNo { get; set; }
    public string CustomerPoNumber { get; set; }
    public DateTime? TaxInvoiceIssueTime { get; set; }
    public System.DateTime SalesDate { get; set; }
    public int OrganizationId { get; set; }
    public string TaxRegisteredName { get; set; }
    public string TaxRegisteredBIN { get; set; }
    public string TaxRegisteredAddress { get; set; }
    public string TaxInvoiceIssueAddress { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public string VatResponsiblePersonSignUrl { get; set; }
    public int? CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerBIN { get; set; }
    public string CustomerAddress { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverContactNo { get; set; }
    public string ShippingAddress { get; set; }
    public int? ShippingCountryId { get; set; }
    public int? VehicleTypeId { get; set; }
    public string VehicleTypeName { get; set; }
    public string VehicleName { get; set; }
    public string VehicleDriverName { get; set; }
    public string VehicleDriverContactNo { get; set; }
    public string VehicleRegNo { get; set; }
    public string VehicleTypeAndRegistrationNo { get; set; }
    public bool? IsTaxInvoicePrined { get; set; }
    public DateTime? TaxInvoicePrintedTime { get; set; }
    public string SalesRemarks { get; set; }
    public string InvoiceStatus { get; set; }
    public string ProductName { get; set; }
    public string Size { get; set; }
    public string BrandName { get; set; }
    public bool IsMeasurable { get; set; }
    public string SKUId { get; set; }
    public string SKUNo { get; set; }
    public string HSCode { get; set; }
    public string ProductCode { get; set; }
    public string ModelNo { get; set; }
    public string MeasurementUnitName { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? ProductPrice { get; set; }
    public decimal? TaxablePrice { get; set; }
    public decimal SupplementaryDutyPercent { get; set; }
    public decimal? ProdSupplementaryDutyAmount { get; set; }
    public decimal VATPercent { get; set; }
    public decimal? ProdVATAmount { get; set; }
    public decimal? ProdPriceInclVATAndDuty { get; set; }

    public string ExportDetails { get; set; }
}