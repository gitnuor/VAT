using System;

namespace vms.entity.StoredProcedureModel.HighValue;

public class SpPurcSalesChallanForHighValueSale
{
    public long? Sl { get; set; }
    public int SalesId { get; set; }
    public string InvoiceNo { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? TaxInvoicePrintedTime { get; set; }
    public System.DateTime SalesDate { get; set; }
    public int? CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public string CustomerBIN { get; set; }
    public decimal TotalPriceWithoutVat { get; set; }
    public decimal? ProdPriceInclVATAndDuty { get; set; }
}