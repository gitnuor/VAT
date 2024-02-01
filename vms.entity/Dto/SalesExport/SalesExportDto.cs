using System;

namespace vms.entity.Dto.SalesExport;

public class SalesExportDto
{
	public int SalesId { get; set; }

	public int? BranchId { get; set; }

	public string BranchName { get; set; }

	public DateTime SalesDate { get; set; }

	public string InvoiceNo { get; set; }

	public DateTime? InvoiceDate { get; set; }

	public string VatChallanNo { get; set; }

	public bool IsVatChallanPrinted { get; set; }

	public string IsVatChallanPrintedStatus { get; set; }

	public DateTime? VatChallanPrintedTime { get; set; }

	public int? CustomerId { get; set; }

	public string CustomerName { get; set; }

	public decimal? TotalPrice { get; set; }
}