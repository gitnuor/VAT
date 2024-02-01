namespace vms.entity.StoredProcedureModel.MushakReturn;

public class MushakReturnPartSix
{
    public decimal DecrementalAdjustmentAmountForVdsPurchase { get; set; }
    public decimal DecrementalAdjustmentAmountForAdvanceTaxInImport { get; set; }
    public decimal DecrementalAdjustmentVatAmountForCreditNote { get; set; }
    public decimal MiscDecrementalAdjustmentAmount { get; set; }
    public string MiscDecrementalAdjustmentDesc { get; set; }
    public decimal TotalDecrementalAdjustmentVatAmount { get; set; }
}