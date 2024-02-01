namespace vms.entity.StoredProcedureModel;

public class SpGetDashBoardInfoDailyPurchase
{
    public int PurchaseDate { get; set; }
    public decimal PurchaseAmount { get; set; }
}

public class SpGetDashBoardInfoSummery
{
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
}