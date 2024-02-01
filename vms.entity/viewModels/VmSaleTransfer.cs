using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.models;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels;

public class VmSaleTransfer
{
    public VmSaleTransfer()
    {
        VmSaleTransferDetails = new List<VmSaleTransferDetail>();
        VmSaleTransferDocuments = new List<VmSaleTransferDocument>();
        VmSaleTransferPayments = new List<VmSaleTransferPayment>();
        CustomerSelectList = new List<SelectListItem>();
        DeliveryMethodSelectList = new List<CustomSelectListItem>();
        ProductForSaleList = new List<SpGetProductForSale>();
        MeasurementUnitSelectList = new List<CustomSelectListItem>();
        DocumentTypeSelectList = new List<CustomSelectListItem>();
        ExportTypeSelectList = new List<SelectListItem>();
        PaymentMethodList = new List<PaymentMethod>();
        BankSelectList = new List<SelectListItem>();
        VehicleTypesList = new List<VehicleType>();
    }
    [DisplayName("From Branch")]
    [Required]
    public int OrgBranchId { get; set; }

    [DisplayName("To Branch")]
    [Required]
    public int ToOrgBranchId { get; set; }
    [DisplayName("Export Type")]
    [Required(ErrorMessage = "Export Type is Required")]
    public int? ExportTypeId { get; set; }
    public int SalesId { get; set; }
    //public decimal? Vdsamount { get; set; }
    [Required(ErrorMessage = "Cus. Name is Required")]
    [DisplayName("Cus. Name")]
    public int? CustomerId { get; set; }
    [Required(ErrorMessage = "Cus. Mobile is Required")]
    [DisplayName("Cus. Mobile")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string CustomerPhoneNumber { get; set; }
    [Required(ErrorMessage = "Order No is Required")]
    [DisplayName("Buy Order No./SO No.")]
    [MaxLength(50, ErrorMessage = "Max Length 50 Allowed")]
    public string WorkOrderNo { get; set; }
    [Required(ErrorMessage = "Sal. Del. Typ. is Required")]
    [DisplayName("Sal. Del. Typ.")]
    public int SalesDeliveryTypeId { get; set; }
    [Required(ErrorMessage = "Del. Date is Required")]
    [DisplayName("Del. Date")]
    public DateTime? DeliveryDate { get; set; }
    [Required(ErrorMessage = "Rec. Name is Required")]
    [DisplayName("Rec. Name")]
    [MaxLength(50)]
    public string ReceiverName { get; set; }
    [Required(ErrorMessage = "Rec. Contact No. is Required")]
    [DisplayName("Rec. Contact No.")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string ReceiverContactNo { get; set; }
    [Required(ErrorMessage = "Del. Address is Required")]
    [DisplayName("Del. Address")]
    [MaxLength(100)]
    public string ShippingAddress { get; set; }
    [Required(ErrorMessage = "Del. Country is Required")]
    [DisplayName("Del. Country")]
    public int? ShippingCountryId { get; set; }
    [Required(ErrorMessage = "PO No.")]
    [DisplayName("PO No.")]
    [MaxLength(20)]
    public string PONo { get; set; }
    [Required(ErrorMessage = "LC No. is Required")]
    [DisplayName("LC No.")]
    [MaxLength(35)]
    public string LcNo { get; set; }
    [Required(ErrorMessage = "LC Date is Required")]
    [DisplayName("LC Date")]
    public DateTime? LcDate { get; set; }
    [Required(ErrorMessage = "Com. Inv. No. is Required")]
    [DisplayName("Com. Inv. No.")]
    [MaxLength(50)]
    public DateTime? BillOfEntry { get; set; }
    [Required(ErrorMessage = "Com. Date is Required")]
    [DisplayName("Com. Date")]
    public DateTime? BillOfEntryDate { get; set; }
    [Required(ErrorMessage = "LC Terms is Required")]
    [DisplayName("LC Terms")]
    [MaxLength(35)]
    public string TermsOfLc { get; set; }
    [Required(ErrorMessage = "Due Date is Required")]
    [DisplayName("Due Date")]
    public DateTime? DueDate { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(500)]
    public string SalesRemarks { get; set; }


    #region Sale Transfer Detail
    public int SalesDetailId { get; set; }
    [Required(ErrorMessage = "Product is Required")]
    [DisplayName("Product")]
    public int ProductId { get; set; }
    [DisplayName("Quantity")]
    [Required(ErrorMessage = "Quantity is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal Quantity { get; set; }
    [Required(ErrorMessage = "Cur. Stock is Required")]
    [DisplayName("Cur. Stock")]
    public decimal CurrentStock { get; set; }

    [DisplayName("M. Unit")]
    public int MeasurementUnitId { get; set; }
    [DisplayName("M. Unit")]
    public string MeasurementUnitName { get; set; }
    [DisplayName("Unit Price")]
    [Required(ErrorMessage = "Unit Price is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.01, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal UnitPrice { get; set; }
    [DisplayName("Total Price")]
    [Required(ErrorMessage = "Total Price is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal TotalPrice { get; set; }
    #endregion

    #region Sale Transfer Document
    [DisplayName("Type")]
    [Required(ErrorMessage = "Document Type is Required")]
    public int DocumentType { get; set; }
    [NotMapped,Required(ErrorMessage ="File is Required")]
    public IFormFile FileUpload { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string DocumentRemarks { get; set; }

    public string FilePath { get; set; }
    #endregion

    #region Sale Transfer Payment
    public int SalePaymentId { get; set; }
    public int SaleId { get; set; }
    [Required(ErrorMessage = "Method is Required")]
    [DisplayName("Method")]
    public int PaymentMethodId { get; set; }
    [Required(ErrorMessage ="Amount is Required")]
    [DisplayName("Amount")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal PaidAmount { get; set; }
    [Required(ErrorMessage = "Payment Date is Requied")]
    [DisplayName("Date")]
    public DateTime PaymentDate { get; set; }
    public long? ApiTransactionId { get; set; }
    [DisplayName("Cheque No.")]
    public string ChequeNo { get; set; }

    [DisplayName("Bank")]
    public int? BankId { get; set; }

    [DisplayName("Wallet No")]
    [MaxLength(20)]
    public string MobilePaymentWalletNo { get; set; }
    [Required(ErrorMessage = "Paid Amount is Required")]

    [DisplayName("Document No./TransactionId")]
    [MaxLength(50)]
    public string DocumentNoOrTransactionId { get; set; }
    [DisplayName("Doc./Trans. Date")]
    public DateTime PaymentDocumentOrTransDate { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string PaymentRemarks { get; set; }
    #endregion

    #region Sale Transfer Vehicle Information
    [DisplayName("Driver Name")]
    [MaxLength(50)]
    public string DriverName { get; set; }
    [DisplayName("Driver Mobile")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string DriverMobile { get; set; }
    [DisplayName("Is Require Registration")]
    public Boolean IsRequireRegistration { get; set; }
    [DisplayName("Vehicle Type")]
    public int VehicleTypeId { get; set; }
    [DisplayName("Vehicle Registration No")]
    [MaxLength(50)]
    public string VehicleRegistrationNo { get; set; }
    #endregion

    public IEnumerable<VmSaleTransferDetail> VmSaleTransferDetails { get; set; }
    public IEnumerable<VmSaleTransferDocument> VmSaleTransferDocuments { get; set; }
    public IEnumerable<VmSaleTransferPayment> VmSaleTransferPayments { get; set; }
    public IEnumerable<SelectListItem> CustomerSelectList { get; set; }
    public IEnumerable<CustomSelectListItem> DeliveryMethodSelectList { get; set; }

    public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; }
    public IEnumerable<SelectListItem> CountrySelectList { get; set; }
    public IEnumerable<SelectListItem> ExportTypeSelectList { get; set; }
    public IEnumerable<SpGetProductForSale> ProductForSaleList { get; set; }
    public IEnumerable<CustomSelectListItem> MeasurementUnitSelectList { get; set; }
    public IEnumerable<CustomSelectListItem> DocumentTypeSelectList { get; set; }
    public IEnumerable<PaymentMethod> PaymentMethodList { get; set; }

    public IEnumerable<SelectListItem> BankSelectList { get; set; }
    public IEnumerable<VehicleType> VehicleTypesList { get; set; }
}

public class VmSaleTransferPost
{
    public VmSaleTransferPost()
    {

        VmSaleTransferDetails = new List<VmSaleTransferDetail>();
        VmSaleTransferDocuments = new List<VmSaleTransferDocument>();
        VmSaleTransferPayments = new List<VmSaleTransferPayment>();
    }

    public bool? IsTaxInvoicePrined { get; set; }
    public int OrganizationId { get; set; }
    [DisplayName("From Branch")]
    [Required]
    public int OrgBranchId { get; set; }

    [DisplayName("To Branch")]
    [Required]
    public int ToOrgBranchId { get; set; }
    public int CreatedBy { get; set; }
    public int SalesTypeId { get; set; }
    public DateTime SalesDate { get; set; }

    public int? ExportTypeId { get; set; }
    public int SalesId { get; set; }
    //public decimal? Vdsamount { get; set; }
    [Required(ErrorMessage = "Cus. Name is Required")]
    [DisplayName("Cus. Name")]
    public int? CustomerId { get; set; }
    [Required(ErrorMessage = "Cus. Mobile is Required")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    [DisplayName("Cus. Mobile")]
    public string CustomerPhoneNumber { get; set; }
    [Required(ErrorMessage = "Order No is Required")]
    [DisplayName("Order No")]
    public string WorkOrderNo { get; set; }
    [Required(ErrorMessage = "Sal. Del. Typ. is Required")]
    [DisplayName("Sal. Del. Typ.")]
    public int SalesDeliveryTypeId { get; set; }
    [Required(ErrorMessage = "Del. Date is Required")]
    [DisplayName("Del. Date")]
    public DateTime? DeliveryDate { get; set; }
    [Required(ErrorMessage = "Rec. Name is Required")]
    [DisplayName("Rec. Name")]
    public string ReceiverName { get; set; }
    [Required(ErrorMessage = "Rec. Contact No. is Required")]
    [DisplayName("Rec. Contact No.")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string ReceiverContactNo { get; set; }
    [Required(ErrorMessage = "Del. Address is Required")]
    [DisplayName("Del. Address")]
    public string ShippingAddress { get; set; }
    [Required(ErrorMessage = "Del. Country is Required")]
    [DisplayName("Del. Country")]
    public int? ShippingCountryId { get; set; }
    [Required(ErrorMessage = "PO No. is Required")]
    [DisplayName("PO No.")]
    [MaxLength(35)]
    public string PONo { get; set; }
    [Required(ErrorMessage = "LC No. is Required")]
    [DisplayName("LC No.")]
    public string LcNo { get; set; }
    [Required(ErrorMessage = "LC Date is Required")]
    [DisplayName("LC Date")]
    public DateTime? LcDate { get; set; }
    [Required(ErrorMessage = "Com. Inv. No.")]
    [DisplayName("Com. Inv. No.")]
    public DateTime? BillOfEntry { get; set; }
    [Required(ErrorMessage = "Com. Date is Required")]
    [DisplayName("Com. Date")]
    public DateTime? BillOfEntryDate { get; set; }
    [Required(ErrorMessage = "LC Terms is Required")]
    [DisplayName("LC Terms")]
    public string TermsOfLc { get; set; }
    [Required(ErrorMessage = "Due Date is Required")]
    [DisplayName("Due Date")]
    public DateTime? DueDate { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(500)]
    public string SalesRemarks { get; set; }

    #region Vehicle Information
    [DisplayName("Driver Name")]
    [MaxLength(50)]
    public string DriverName { get; set; }
    [DisplayName("Driver Mobile")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string DriverMobile { get; set; }
    [DisplayName("Is Require Registration")]
    public Boolean IsRequireRegistration { get; set; }
    [DisplayName("Vehicle Type")]
    public int VehicleTypeId { get; set; }
    [DisplayName("Vehicle Registration No")]
    [MaxLength(50)]
    public string VehicleRegistrationNo { get; set; }
    #endregion

    public IEnumerable<VmSaleTransferDetail> VmSaleTransferDetails { get; set; }
    public IEnumerable<VmSaleTransferDocument> VmSaleTransferDocuments { get; set; }
    public IEnumerable<VmSaleTransferPayment> VmSaleTransferPayments { get; set; }

}
public class VmSaleTransferDetail
{
    public int SalesDetailId { get; set; }
    public int SalesId { get; set; }
    [DisplayName("VAT Type")]
    public int ProductVatTypeId => 1;
    [Required(ErrorMessage = "Product is Required")]
    [DisplayName("Product")]
    public int ProductId { get; set; }
    [Required(ErrorMessage = "Quantity is Required")]
    [DisplayName("Quantity")]
    public decimal Quantity { get; set; }
    [Required(ErrorMessage = "Cur. Stock is Required")]
    [DisplayName("Cur. Stock")]
    public decimal CurrentStock { get; set; }

    [DisplayName("M. Unit")]
    public int MeasurementUnitId { get; set; }
    [DisplayName("M. Unit")]
    public string MeasurementUnitName { get; set; }
    [Required(ErrorMessage = "Unit Price is Required")]
    [DisplayName("Unit Price")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Maximum 4 digit after decimal is allowed.")]
    public string UnitPrice { get; set; }
    [Required]
    [DisplayName("Total Price")]
    public double TotalPrice { get; set; }
        
}
public class VmSaleTransferDocument
{
    [DisplayName("Type")]
    public int DocumentType { get; set; }
    [NotMapped]
    public IFormFile FileUpload { get; set; }
    public string FileName => FileUpload.FileName;
    public string FileUrl => FileUpload.FileName;
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string DocumentRemarks { get; set; }

    public string FilePath { get; set; }
}
public class VmSaleTransferPayment
{
    public int SalePaymentId { get; set; }
    public int SaleId { get; set; }
    [Required(ErrorMessage = "Method is Required")]
    [DisplayName("Method")]
    public int PaymentMethodId { get; set; }
    [Required(ErrorMessage = "Amount is Required")]
    [DisplayName("Amount")]
    public decimal PaidAmount { get; set; }
    [Required(ErrorMessage = "Date is Required")]
    [DisplayName("Date")]
    public DateTime PaymentDate { get; set; }
    public long? ApiTransactionId { get; set; }
    [DisplayName("Cheque No.")]
    public string ChequeNo { get; set; }

    [DisplayName("Bank")]
    public int? BankId { get; set; }

    [DisplayName("Wallet No")]
    [MaxLength(20)]
    public string MobilePaymentWalletNo { get; set; }
    [Required(ErrorMessage = "Paid Amount is Required")]

    [DisplayName("Document No./TransactionId")]
    [MaxLength(50)]
    public string DocumentNoOrTransactionId { get; set; }
    [DisplayName("Doc./Trans. Date")]
    public DateTime PaymentDocumentOrTransDate { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string PaymentRemarks { get; set; }
}