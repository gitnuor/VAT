using System;

namespace vms.entity.StoredProcedureModel.MushakReturn;

public class MushakReturnSubFormUma
{
    public long? SlNo { get; set; }
    public int SalesId { get; set; }
    public string CustomerBinNo { get; set; }
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public decimal? Price { get; set; }
    public decimal? DeductedVat { get; set; }
    public string InvoiceNo { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime SalesDate { get; set; }
    public DateTime? TaxInvoicePrintedTime { get; set; }
    public string VDSCertificateNo { get; set; }
    public DateTime? VDSCertificateIssueTime { get; set; }
    public string EconomicCode { get; set; }
    public string VDSPaymentBookTransferNo { get; set; }
    public DateTime? VDSPaymentDate { get; set; }
    public string Note { get; set; }
}