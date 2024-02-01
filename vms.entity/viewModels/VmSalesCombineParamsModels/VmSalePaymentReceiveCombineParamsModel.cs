using System;

namespace vms.entity.viewModels.VmSalesCombineParamsModels;

public class VmSalePaymentReceiveCombineParamsModel
{
    public int SalesPaymentReceiveId { get; set; }
    public int SalesId { get; set; }
    public int ReceivedPaymentMethodId { get; set; }
    public decimal ReceiveAmount { get; set; }
    public DateTime ReceiveDate { get; set; }
    public string ReferenceKey { get; set; }
    public long? ApiTransactionId { get; set; }

    public string DocumentNoOrTransactionId { get; set; }
    public DateTime PaymentDocumentOrTransDate { get; set; }
    public string MobilePaymentWalletNo { get; set; }
    public int? BankId { get; set; }
    public string PaymentRemarks { get; set; }

}