using System;

namespace vms.entity.viewModels.VmPurchaseCombineParamsModels;

public class PurchasePaymentCombinedParamModel
{
    public int PurchasePaymentId { get; set; }
    public int PurchaseId { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string ReferenceKey { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public long? ApiTransactionId { get; set; }

    public string DocumentNoOrTransactionId { get; set; }
    public DateTime PaymentDocumentOrTransDate { get; set; }
    public string MobilePaymentWalletNo { get; set; }
    public int? BankId { get; set; }
    public string PaymentRemarks { get; set; }


}