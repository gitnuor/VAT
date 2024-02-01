using System;

namespace vms.entity.StoredProcedureModel;

public class SpGetCreditNoteMushakForSalesPriceReduce
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerBinOrNid { get; set; }
    public string CustomerAddress { get; set; }
    public string SalesShippingAddress { get; set; }
    public string SalesInvoiceNo { get; set; }
    public string SalesVatChallanNo { get; set; }
    public DateTime? SalesVatChallanNoPrintedTime { get; set; }
    public DateTime SalesDate { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationBin { get; set; }
    public string OrganizationAddress { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public int SalesPriceAdjustmentId { get; set; }
    public string CreditNoteChallanNo { get; set; }
    public bool IsCreditNoteChallanPrinted { get; set; }
    public string InvoiceStatus { get; set; }
    public DateTime? CreditNoteChallanPrintTime { get; set; }
    public DateTime? CreditNoteCreateTime { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductHsCode { get; set; }
    public string ModelNo { get; set; }
    public string ReasonOfChange { get; set; }
    public decimal UnitPrice { get; set; }
    public string MeasurementUnitName { get; set; }
    public decimal SalesQuantity { get; set; }
    public decimal? SalesPrice { get; set; }
    public decimal? SalesVatAmount { get; set; }
    public decimal? SalesSupplementaryDutyAmount { get; set; }
    public decimal ReturnQuantity { get; set; }
    public decimal? ReturnedPrice { get; set; }
    public decimal? ReturnedVatAmount { get; set; }
    public decimal? ReturnedSupplementaryDutyAmount { get; set; }
    public decimal? DeductionAmount { get; set; }
    public decimal SupplementaryDutyPercent { get; set; }
    public decimal VATPercent { get; set; }
    public string VehicleTypeName { get; set; }
    public string VehicleName { get; set; }
    public string VehicleDriverName { get; set; }
    public string VehicleDriverContactNo { get; set; }
    public string VehicleRegNo { get; set; }
    public string VehicleTypeAndRegistrationNo { get; set; }
    public string VatResponsiblePersonSignUrl { get; set; }
}