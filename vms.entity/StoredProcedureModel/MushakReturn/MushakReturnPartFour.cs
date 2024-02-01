namespace vms.entity.StoredProcedureModel.MushakReturn;

public class MushakReturnPartFour
{
    public decimal ZeroVatProdLocalPurchaseAmount { get; set; }
    public decimal ZeroVatProdImportAmount { get; set; }
    public decimal VatExemptedProdLocalPurchaseAmount { get; set; }
    public decimal VatExemptedProdImportAmount { get; set; }
    public decimal StandardVatProdLocalPurchaseAmount { get; set; }
    public decimal StandardVatProdLocalPurchaseVatAmount { get; set; }
    public decimal StandardVatProdImportAmount { get; set; }
    public decimal StandardVatProdImportVatAmount { get; set; }
    public decimal OtherThanStandardVatProdLocalPurchaseAmount { get; set; }
    public decimal OtherThanStandardVatProdLocalPurchaseVatAmount { get; set; }
    public decimal OtherThanStandardVatProdImportAmount { get; set; }
    public decimal OtherThanStandardVatProdImportVatAmount { get; set; }
    public decimal FixedVatProdLocalPurchaseAmount { get; set; }
    public decimal FixedVatProdLocalPurchaseVatAmount { get; set; }
    public decimal NonRebatableProdLocalPurchaseFromTurnOverOrgAmount { get; set; }
    public decimal NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount { get; set; }
    public decimal NonRebatableProdLocalPurchaseFromNonRegOrgAmount { get; set; }
    public decimal NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount { get; set; }
    public decimal NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount { get; set; }
    public decimal NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount { get; set; }
    public decimal NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount { get; set; }
    public decimal NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount { get; set; }
    public decimal TotalRawMaterialPurchaseAmount { get; set; }
    public decimal TotalRawMaterialRebateAmount { get; set; }
}