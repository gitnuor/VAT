namespace vms.entity.StoredProcedureModel.MushakReturn;

public class MushakReturnPartSeven
{
    public decimal NetTaxTotalPayableVatAmountInCurrentTaxTerm { get; set; }
    public decimal NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance { get; set; }
    public decimal NetTaxTotalPayableSdAmountInCurrentTaxTerm { get; set; }
    public decimal NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance { get; set; }
    public decimal NetTaxSdAmountForDebitNote { get; set; }
    public decimal NetTaxSdAmountForCreditNote { get; set; }
    public decimal NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd { get; set; }
    public decimal NetTaxInterstAmountForDeuVat { get; set; }
    public decimal NetTaxInterstAmountForDeuSuppDuty { get; set; }
    public decimal NetTaxFineAndPenaltyForDelaySubmission { get; set; }
    public decimal NetTaxFineAndPenaltyOthers { get; set; }
    public decimal NetTaxExciseDuty { get; set; }
    public decimal NetTaxDevelopmentSurcharge { get; set; }
    public decimal NetTaxInformationTechnologyDevelopmentSurcharge { get; set; }
    public decimal NetTaxHealthCareSurcharge { get; set; }
    public decimal NetTaxEnvironmentProtectionSurcharge { get; set; }
    public decimal NetPayableVATForTreasurySubmission { get; set; }
    public decimal NetPayableSDForTreasurySubmission { get; set; }
    public decimal NetTaxVatEndingBalanceOfLastTerm { get; set; }
    public decimal NetTaxSdEndingBalanceOfLastTerm { get; set; }
}