using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace vms.entity.viewModels
{
    public abstract class VmSaleLocalBase
    {
        public string OrganizationName { get; set; }
        public string OrganizationBin { get; set; }
        public string OrganizationAddress { get; set; }
        public string VatResponsiblePersonName { get; set; }
        public string VatResponsiblePersonDesignation { get; set; }
        public int SalesId { get; set; }
        [DisplayName("Branch")]
        [Required]
        public int OrgBranchId { get; set; }
        [DisplayName("Is VDS?")]
        public bool IsVatDeductedInSource { get; set; }
        [DisplayName("VDS Amount")]
        [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Maximum 8 digit after decimal is allowed.")]
        public decimal? VdsAmount { get; set; }
        [Required(ErrorMessage = "Customer Name is Required")]
        [DisplayName("Customer Name")]
        public int? CustomerId { get; set; }
        [Required(ErrorMessage = "Customer Mobile is Required")]
        [DisplayName("Customer Mobile")]
        [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
        public string CustomerPhoneNumber { get; set; }
        [Required(ErrorMessage = "Sal. Del. Typ. is Required")]
        [DisplayName("Delivery Type")]
        public int SalesDeliveryTypeId { get; set; }
        [Required(ErrorMessage = "Del. Date is Required")]
        [DisplayName("Delivery Date")]
        public DateTime? DeliveryDate { get; set; }
        // [Required(ErrorMessage = "Rec. Name is Required")]
        [DisplayName("Receiver Name")]
        [MaxLength(50)]
        public string ReceiverName { get; set; }
        // [Required(ErrorMessage = "Rec. Contact No. is Required")]
        [DisplayName("Recceiver Contact No.")]
        [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
        public string ReceiverContactNo { get; set; }
        [Required(ErrorMessage = "Del. Address is Required")]
        [DisplayName("Delivery Address")]
        [MaxLength(200)]
        public string ShippingAddress { get; set; }
        [DisplayName("Remarks")]
        [MaxLength(500)]
        public string SalesRemarks { get; set; }

        [MaxLength(50, ErrorMessage = "Max Length 50 allowed")]
        public string InvoiceNo { get; set; }

        public DateTime? InvoiceDate { get; set; }
        [DisplayName("Sales Breakdown")]
        [NotMapped, Required(ErrorMessage = "File is Required")]
        public IFormFile FileSalesBreakDown { get; set; }


        #region SaleLocal Detail

        public bool IsImposeServiceCharge { get; set; }
        public decimal DefaultServiceChargePercent { get; set; }
        [DisplayName("Service Charge (%)")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
        [Range(0, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal ServiceChargePercent { get; set; }

        public int SalesDetailId { get; set; }
        [Required(ErrorMessage = "Product is Required")]
        [DisplayName("Product")]
        public int ProductId { get; set; }

        [DisplayName("Description")]
        [MaxLength(500)]
        public string ProductDescription { get; set; }

        [DisplayName("HS Code")]
        [MaxLength(50)]
        public string Hscode { get; set; }

        [DisplayName("Product Code")]
        [MaxLength(50)]
        public string ProductCode { get; set; }

        [DisplayName("Part No.")]
        [MaxLength(100)]
        public string PartNo { get; set; }

        [DisplayName("SKU")]
        [MaxLength(50)]
        public string SKUNo { get; set; }

        [DisplayName("SKU ID")]
        [MaxLength(50)]
        public string SKUId { get; set; }

        [DisplayName("Goods ID")]
        [MaxLength(50)]
        public string GoodsId { get; set; }
        [Required(ErrorMessage = "VAT Type is Required")]
        [DisplayName("VAT Type")]
        public int ProductVatTypeId { get; set; }
        public long? ProductTransactionBookId { get; set; }
        [DisplayName("Quantity")]
        [Required(ErrorMessage = "Quantity is Required")]
        [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
        public decimal Quantity { get; set; }
        [Required(ErrorMessage = "Current Stock is Required")]
        [DisplayName("Cur. Stock")]
        public decimal CurrentStock { get; set; }

        [DisplayName("Unit Price")]
        [Required(ErrorMessage = "Unit Price is Required")]
        [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
        [Range(0.00000001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Disc./Item")]
        [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
        [Range(0, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal DiscountPerItem { get; set; }
        [DisplayName("VAT Percent")]
        [RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "Maximum 2 digit after decimal is allowed.")]
        [Range(0, 5000)]
        public decimal ProductVatPercent { get; set; }
        [Required(ErrorMessage = "SD (%) is Required")]
        [RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "Maximum 2 digit after decimal is allowed.")]
        [DisplayName("SD (%)")]
        public decimal SupplementaryDutyPercent { get; set; }
        [DisplayName("M. Unit")]
        public int MeasurementUnitId { get; set; }
        [DisplayName("M. Unit")]
        public string MeasurementUnitName { get; set; }
        [Required(ErrorMessage = "Rate is Required")]
        [DisplayName("Rate")]
        public string Rate { get; set; }
        [DisplayName("Total Price")]
        [Required(ErrorMessage = "Total Price is Required")]
        [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal TotalPrice { get; set; }

        [DisplayName("Service Charge")]
        [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
        [Range(0, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal TotalServiceCharge { get; set; }

        [DisplayName("Taxable Price")]
        [Required(ErrorMessage = "Taxable Price is Required")]
        [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal TaxablePrice { get; set; }

        [Required(ErrorMessage = "Vat is Required")]
        [DisplayName("VAT")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Maximum 4 digit after decimal is allowed.")]
        public double Vat { get; set; }
        [DisplayName("Total VAT")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
        [Range(0, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public double TotalVat { get; set; }

        //Extra Field Add Here
        [DisplayName("Prod. Disc.")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
        [Range(0, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal TotalProductDiscount { get; set; }

        [DisplayName("Price (-Disc.)")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal ProductPriceWithDiscount { get; set; }

        [DisplayName("SD")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
        [Required(ErrorMessage = "SD is required")]
        [Range(0, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal ProductSupplementaryDuty { get; set; }

        [DisplayName("Price for VAT")]
        [Required(ErrorMessage = "VAT-able Price is required")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal VatAblePrice { get; set; }

        [DisplayName("Price (+SD+VAT)")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal ProductPriceWithVat { get; set; }

        [DisplayName("Price (+SD+VAT-Disc.)")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal ProductPriceWithVatAfterDiscount { get; set; }

        [DisplayName("Total VAT")]
        [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
        [Range(0, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
        public decimal ProductVat { get; set; }

        //Extra Field End Here
        #endregion


        #region Sale Local Document

        [DisplayName("Type")]
        [Required(ErrorMessage = "DocumentType is Required")]
        public int DocumentType { get; set; }
        [NotMapped, Required(ErrorMessage = "File is Required")]
        public IFormFile FileUpload { get; set; }

        public string FilePath { get; set; }
        [DisplayName("Remarks")]
        [MaxLength(50)]
        public string DocumentRemarks { get; set; }
        #endregion

        #region SaleLocal Payment
        public int SalePaymentId { get; set; }
        public int SaleId { get; set; }
        [Required(ErrorMessage = "Payment Method Is Required")]
        [DisplayName("Method")]
        public int PaymentMethodId { get; set; }

        [DisplayName("Bank")]
        public int? BankId { get; set; }

        [DisplayName("Wallet No")]
        [MaxLength(20)]
        public string MobilePaymentWalletNo { get; set; }
        [Required(ErrorMessage = "Paid Amount is Required")]
        [DisplayName("Paid Amount")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
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
        #endregion



    }
}
