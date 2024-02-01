using vms.entity.viewModels.BranchTransferSend;

namespace vms.entity.viewModels.MushakViewModel;

public class TransferChallanViewModel
{
	public SpGetBranchTransferChallanModel ChallanModel { get; set; }
	public int Language { get; set; }
	public string InvoiceNameEng { get; set; }
	public string InvoiceNameBan { get; set; }
	public bool IsGovtLogoRequired { get; set; } = true;
	public bool IsSaleSimplified { get; set; } = false;
	public bool IsBuyOrderNumberRequired { get; set; } = false;
	public bool IsPoNumberRequired { get; set; } = false;
	public bool IsSalesInvoiceNoRequired { get; set; } = false;
	public bool IsSalesInvoiceDateRequired { get; set; } = false;
	public bool IsSalesDateRequired { get; set; } = false;
}