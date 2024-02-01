using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;
using vms.entity.Utility;

namespace vms.entity.viewModels;

public class VmDuePayment
{
    public VmDuePayment()
    {
        BankSelectList = new List<CustomSelectListItem>();
        PaymentMethodList = new List<PaymentMethod>();
    }
    public int PurchaseId { get; set; }
    public int SalesId { get; set; }
    [Display(Name = "Payment Amount")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal PaidAmount { get; set; }
    public decimal? DueAmount { get; set; }
    public decimal? PayableAmount { get; set; }
    public decimal? PrevPaidAmount { get; set; }
    [Display(Name = "Payment Method")]
    [Required]
    public int PaymentMethodId { get; set; }
    public IEnumerable<CustomSelectListItem> PaymentMethods;
    public IEnumerable<PaymentMethod> PaymentMethodList { get; set; }
    public IEnumerable<CustomSelectListItem> BankSelectList { get; set; }

    //Payment Extra Field

    [DisplayName("Bank")]
    public int? BankId { get; set; }
    [DisplayName("Wallet No")]
    [MaxLength(20)]
    public string MobilePaymentWalletNo { get; set; }
    [Required(ErrorMessage = "Payment Date is Required")]
    [DisplayName("Payment Date")]
    public DateTime PaymentDate { get; set; }

    [DisplayName("Document No./TransactionId")]
    [MaxLength(50)]
    public string DocumentNoOrTransactionId { get; set; }
    [DisplayName("Doc./Trans. Date")]
    public DateTime PaymentDocumentOrTransDate { get; set; }
}