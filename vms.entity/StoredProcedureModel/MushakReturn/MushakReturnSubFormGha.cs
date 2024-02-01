using System;

namespace vms.entity.StoredProcedureModel.MushakReturn;

public class MushakReturnSubFormGha
{
    public long? SlNo { get; set; }
    public int PurchaseId { get; set; }
    public string VendorBinNo { get; set; }
    public string VendorName { get; set; }
    public string VendorAddress { get; set; }
    public decimal? Price { get; set; }
    public decimal? DeductedVat { get; set; }
    public string InvoiceNo { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime TaxInvoicePrintedTime { get; set; }
    public string VDSCertificateNo { get; set; }
    public DateTime? VDSCertificateDate { get; set; }
    public string EconomicCode { get; set; }
    public string VDSPaymentBookTransferNo { get; set; }
    public DateTime? VDSPaymentDate { get; set; }
    public string Note { get; set; }
}