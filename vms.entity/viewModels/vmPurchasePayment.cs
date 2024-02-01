using System;

namespace vms.entity.viewModels;

public class vmPurchasePayment
{
    public int PurchaseId { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal PaidAmount { get; set; }
    public int CreatedBy { get; set; }


    public int? BankId { get; set; }
    public string MobilePaymentWalletNo { get; set; }
    public DateTime PaymentDate { get; set; }
    public string DocumentNoOrTransactionId { get; set; }
    public DateTime PaymentDocumentOrTransDate { get; set; }
    public DateTime CreatedTime { get; set; }

    public string BankAccountNo { get; set; }
    public string ReferenceKey { get; set; }
    public string PaymentRemarks { get; set; }
}
public class VmSalesPaymentReceive
{
    public int SalesId { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal PaidAmount { get; set; }
    public int CreatedBy { get; set; }


    public int? BankId { get; set; }
    public string MobilePaymentWalletNo { get; set; }
    public DateTime PaymentDate { get; set; }
    public string DocumentNoOrTransactionId { get; set; }
    public DateTime PaymentDocumentOrTransDate { get; set; }
    public DateTime CreatedTime { get; set; }

    public string BankAccountNo { get; set; }
    public string ReferenceKey { get; set; }
    public string PaymentRemarks { get; set; }
}