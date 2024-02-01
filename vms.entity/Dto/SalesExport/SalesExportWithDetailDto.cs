using System;
using System.Collections.Generic;

namespace vms.entity.Dto.SalesExport;

public class SalesExportWithDetailDto
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

	public IEnumerable<SalesExportDetailDto> Details { get; set; } = new List<SalesExportDetailDto>();
}