namespace vms.entity.StoredProcedureModel;

using System;
using System.ComponentModel.DataAnnotations;

public class SpDebiNotetMushak
{
    [Key]
    public int VendorId { get; set; }
    public string VendorName { get; set; }
    public string VendorBinOrNid { get; set; }
    public string VendorAddress { get; set; }
    public string PurchaseInvoiceNo { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string PurchaseVatChallanNo { get; set; }
    public DateTime? PurchaseVatChallanIssueDate { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationBin { get; set; }
    public string OrganizationAddress { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public int DebitNoteId { get; set; }
    public string DebitNoteChallanNo { get; set; }
    public bool IsDebitNoteChallanPrinted { get; set; }
    public string InvoiceStatus { get; set; }
    public DateTime? DebitNoteChallanPrintTime { get; set; }
    public DateTime? DebitNoteCreateTime { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductHsCode { get; set; }
    public string ModelNo { get; set; }
    public string ReasonOfReturn { get; set; }
    public decimal UnitPrice { get; set; }
    public string MeasurementUnitName { get; set; }
    public decimal PurchaseQuantity { get; set; }
    public decimal? PurchasePrice { get; set; }
    public decimal? PurchaseVatAmount { get; set; }
    public decimal? PurchaseSupplementaryDutyAmount { get; set; }
    public decimal ReturnQuantity { get; set; }
    public decimal? ReturnedPrice { get; set; }
    public decimal? ReturnVatAmount { get; set; }
    public decimal? ReturnSupplementaryDutyAmount { get; set; }
    public decimal SupplementaryDutyPercent { get; set; }
    public decimal VATPercent { get; set; }
    public decimal? DeductionAmount { get; set; }
    public string VehicleTypeName { get; set; }
    public string VehicleName { get; set; }
    public string VehicleDriverName { get; set; }
    public string VehicleDriverContactNo { get; set; }
    public string VehicleRegNo { get; set; }
    public string VehicleTypeAndRegistrationNo { get; set; }
}