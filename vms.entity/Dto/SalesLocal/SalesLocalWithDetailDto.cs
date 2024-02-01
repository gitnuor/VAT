using System;
using System.Collections.Generic;

namespace vms.entity.Dto.SalesLocal;

public class SalesLocalWithDetailDto
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

	public decimal? TotalPriceWithoutTax { get; set; }

	public decimal? TotalSupplementaryDuty { get; set; }

	public decimal? TotalVat { get; set; }

	public bool IsVds { get; set; }

	public string IsVdsStatus { get; set; }

	public decimal? Vdsamount { get; set; }

	public bool? IsTds { get; set; }

	public string IsTdsStatus { get; set; }

	public decimal? TdsAmount { get; set; }

	public decimal? TotalReceivableAmount { get; set; }
	public IEnumerable<SalesLocalDetailDto> Details { get; set; } = new List<SalesLocalDetailDto>();
}