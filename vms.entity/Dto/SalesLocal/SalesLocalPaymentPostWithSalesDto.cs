using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.SalesLocal;

public class SalesLocalPaymentPostWithSalesDto
{
	[Required(ErrorMessage = "{0} Is Required")]
	public string SalePaymentId { get; set; }
	[Required(ErrorMessage = "{0} Is Required")]
	[DisplayName("Method")]
	public string PaymentMethodId { get; set; }

	[DisplayName("Bank")]
	public string BankId { get; set; }

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