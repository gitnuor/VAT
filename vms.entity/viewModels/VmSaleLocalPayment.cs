using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class VmSaleLocalPayment
{
    public int SalePaymentId { get; set; }
    public int SaleId { get; set; }
    [Required(ErrorMessage = "Payment Method Is Required")]
    [DisplayName("Method")]
    public int PaymentMethodId { get; set; }

    [DisplayName("Bank")]
    public int? BankId { get; set; }

    public bool IsBankingChannel { get; set; }
    public bool IsMobileTransaction { get; set; }

    [DisplayName("Wallet No")]
    [MaxLength(20)]
    public string MobilePaymentWalletNo { get; set; }
    [Required(ErrorMessage = "Paid Amount is Required")]
    [DisplayName("Paid Amount")]
    public decimal PaidAmount { get; set; }
    [Required(ErrorMessage = "Payment Date is Required")]
    [DisplayName("Payment Date")]
    public DateTime PaymentDate { get; set; }

    [DisplayName("Document No./TransactionId")]
    [MaxLength(50)]
    public string DocumentNoOrTransactionId { get; set; }
    [DisplayName("Doc./Trans. Date")]
    public DateTime PaymentDocumentOrTransDate { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string PaymentRemarks { get; set; }
}