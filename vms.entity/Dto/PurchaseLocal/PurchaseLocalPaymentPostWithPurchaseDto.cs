using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.Utility;

namespace vms.entity.Dto.PurchaseLocal;

public class PurchaseLocalPaymentPostWithPurchaseDto
{
    [Required]
    [DisplayName("Purchase Payment")]
    public string PurchasePaymentId { get; set; }

    [DisplayName("Payment Method")]
    [Required(ErrorMessage = "Payment Method is Required")]
    public string PaymentMethodId { get; set; }

    [DisplayName("Bank")]
    public string BankId { get; set; }

    [DisplayName("Wallet No")]
    [MaxLength(20)]
    public string MobilePaymentWalletNo { get; set; }

    [Required(ErrorMessage = "Paid Amount is Required")]
    [DisplayName("Paid Amount")]
    [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
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