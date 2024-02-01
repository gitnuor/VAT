using System;

namespace vms.entity.StoredProcedureModel.HighValue;

public class SpPurcSalesChallanForHighValuePurchase
{

    public long? Sl { get; set; }
    public int PurchaseId { get; set; }
    public string VendorInvoiceNo { get; set; }
    public System.DateTime PurchaseDate { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? VatChallanIssueDate { get; set; }
    public string VendorName { get; set; }
    public string VendorAddress { get; set; }
    public string VendorBinOrNid { get; set; }
    public string TaxRegisteredName { get; set; }
    public string TaxRegisteredBIN { get; set; }
    public string TaxInvoiceIssueAddress { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public decimal TotalPriceWithoutVat { get; set; }
    public decimal? ProdPriceInclVATAndDuty { get; set; }
}