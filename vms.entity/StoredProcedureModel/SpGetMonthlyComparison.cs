namespace vms.entity.StoredProcedureModel;

public class SpGetMonthlyComparison
{
	public string OrganizationId { get; set; }
	public string OrganizationName { get; set; }
	public string OrganizationAddress { get; set; }
	public string OrganizationBin { get; set; }
	public int ComparisonYear { get; set; }
	public int ComparisonMonth { get; set; }
	public string MonthName { get; set; }
	public decimal TotalPurchase { get; set; }
	public decimal TotalPurchaseVat { get; set; }
	public decimal TotalVdsPurchase { get; set; }
	public decimal TotalVdsPurchaseVdsAmount { get; set; }
	public decimal TotalDebitNote { get; set; }
	public decimal TotalDebitNoteVat { get; set; }
	public decimal TotalSales { get; set; }
	public decimal TotalSalesVat { get; set; }
	public decimal TotalVdsSales { get; set; }
	public decimal TotalVdsSalesVdsAmount { get; set; }
	public decimal TotalCreditNote { get; set; }
	public decimal TotalCreditNoteVat { get; set; }
	public decimal TotalInputVat { get; set; }
	public decimal TotalOutputVat { get; set; }
	public decimal TotalVat { get; set; }
}