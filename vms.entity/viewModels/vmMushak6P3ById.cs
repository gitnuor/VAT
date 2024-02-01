using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels;

public class vmMushak6P3ById
{
    public List<SpSalesTaxInvoice> SalesTaxInvoices { get; set; }
    public int Language { get; set; }
    
    public string InvoiceNameEng { get; set; }
    public string InvoiceNameBan { get; set; }
    public bool IsGovtLogoRequired { get; set; } = true;
    public bool IsSaleSimplified { get; set; } = false;
    public bool IsBuyOrderNumberRequired { get; set; } = false;
	public bool IsPoNumberRequired { get; set; } = false;
    public bool IsSalesInvoiceNoRequired { get; set; } = true;
    public bool IsSalesInvoiceDateRequired { get; set; } = false;
    public bool IsEmhCodeRequired { get; set; } = false;
    public bool IsSalesDateRequired { get; set; } = false;
}