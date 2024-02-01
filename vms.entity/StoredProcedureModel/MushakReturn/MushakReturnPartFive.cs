namespace vms.entity.StoredProcedureModel.MushakReturn;

public class MushakReturnPartFive
{
    public decimal IncrementalAdjustmentAmountForVdsSale { get; set; }
    public decimal IncrementalAdjustmentAmountForNotPaidInBankingChannel { get; set; }
    public decimal IncrementalAdjustmentVatAmountForDebitNote { get; set; }
    public decimal MiscIncrementalAdjustmentAmount { get; set; }
    public string MiscIncrementalAdjustmentDesc { get; set; }
    public decimal TotalIncrementalAdjustmentVatAmount { get; set; }
}