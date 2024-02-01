using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.PurchaseImport;

public class PurchaseImportPostDto
{
	[DisplayName("Branch")] [Required] public string BranchId { get; set; }

	[DisplayName("Purchase")] public string PurchaseId { get; set; }

	[DisplayName("Reason")]
	[Required(ErrorMessage = "Reason is Required")]
	public string PurchaseReasonId { get; set; }

	[DisplayName("P/O No.")] public string PoNumber { get; set; }

	[DisplayName("Vendor")]
	[Required(ErrorMessage = "{0} is Required")]
	public string VendorId { get; set; }

	[DisplayName("Comm. Inv. No.")] public string CommercialInvoiceNo { get; set; }

	[DisplayName("Comm. Inv. Date")] public DateTime? CommercialInvoiceDate { get; set; }

	[DisplayName("Invoice No")] public string InvoiceNo { get; set; }

	[DisplayName("Invoice Date")] public DateTime? InvoiceDate { get; set; }

	[DisplayName("Voucher No")] public string VoucherNo { get; set; }

	[DisplayName("Voucher Date")] public DateTime? VoucherDate { get; set; }

	[DisplayName("Purchase Date")] public DateTime PurchaseDate { get; set; }

	[DisplayName("Lc No.")] public string LcNo { get; set; }
	[DisplayName("Lc Date")] public DateTime LcDate { get; set; }
	[DisplayName("Bill Of Entry")] public string BillOfEntry { get; set; }
	[DisplayName("BOE Date")] public DateTime BillOfEntryDate { get; set; }
	[DisplayName("Due Date")] public DateTime? DueDate { get; set; }
	[DisplayName("Terms Of Lc")] public string TermsOfLc { get; set; }

	public IEnumerable<PurchaseImportDetailPostWithPurchaseDto> Details { get; set; } = new List<PurchaseImportDetailPostWithPurchaseDto>();
	public IEnumerable<DocumentPostWithObjectDto> Documents { get; set; } = new List<DocumentPostWithObjectDto>();
	public IEnumerable<PurchaseImportTaxPaymentPostWithPurchaseDto> ImportTaxPayments { get; set; } = new List<PurchaseImportTaxPaymentPostWithPurchaseDto>();
}