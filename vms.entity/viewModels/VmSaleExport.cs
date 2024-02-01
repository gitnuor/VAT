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

public class VmSaleExport
{
    public VmSaleExport()
    {
        VmSaleExportDetails = new List<VmSaleExportDetail>();
        VmSaleLocalDocuments = new List<VmSaleLocalDocument>();
        VmSaleLocalPayments = new List<VmSaleLocalPayment>();
        CustomerList = new List<Customer>();
        DeliveryMethodSelectList = new List<CustomSelectListItem>();
        ProductForSaleList = new List<SpGetProductForSale>();
        MeasurementUnitSelectList = new List<CustomSelectListItem>();
        DocumentTypeSelectList = new List<CustomSelectListItem>();
        ExportTypeSelectList = new List<CustomSelectListItem>();
        PaymentMethodList = new List<PaymentMethod>();
        BankSelectList = new List<CustomSelectListItem>();
        VehicleTypesList = new List<VehicleType>();
    }
    [DisplayName("Branch")]
    [Required]
    public int OrgBranchId { get; set; }
    [DisplayName("Export Type")]
    // [Required(ErrorMessage = "Export Type is Required")]
    public int? ExportTypeId { get; set; }
    public int SalesId { get; set; }
    //public decimal? Vdsamount { get; set; }
    [Required(ErrorMessage = "Customer Name is Required")]
    [DisplayName("Customer Name")]
    public int? CustomerId { get; set; }
    [Required(ErrorMessage = "Customer Mobile is Required")]
    [DisplayName("Customer Mobile")]
    // [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string CustomerPhoneNumber { get; set; }
    // [Required(ErrorMessage = "Order No is Required")]
    [DisplayName("Buy Order No/SO No")]
    [MaxLength(50, ErrorMessage = "Max Length 50 Allowed")]
    public string WorkOrderNo { get; set; }
    [Required(ErrorMessage = "Delivery Typ. is Required")]
    [DisplayName("Delivery Typ.")]
    public int SalesDeliveryTypeId { get; set; }
    [Required(ErrorMessage = "Delivery Date is Required")]
    [DisplayName("Delivery Date")]
    public DateTime? DeliveryDate { get; set; }
    // [Required(ErrorMessage = "Rec. Name is Required")]
    [DisplayName("Receiver Name")]
    [MaxLength(50)]
    public string ReceiverName { get; set; }
    // [Required(ErrorMessage = "Rec. Contact No. is Required")]
    [DisplayName("Receiver Contact No.")]
    // [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string ReceiverContactNo { get; set; }
    // [Required(ErrorMessage = "Delivery Address is Required")]
    [DisplayName("Delivery Address")]
    [MaxLength(100)]
    public string ShippingAddress { get; set; }
    // [Required(ErrorMessage = "Delivery Country is Required")]
    [DisplayName("Delivery Country")]
    public int? ShippingCountryId { get; set; }
    // [Required(ErrorMessage = "PO No.")]
    [DisplayName("PO No.")]
    [MaxLength(20)]
    public string PONo { get; set; }
    // [Required(ErrorMessage = "LC No. is Required")]
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
    [Required(ErrorMessage = "Com. Inv. Date is Required")]
    [DisplayName("Com. Inv. Date")]
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


    #region Sale Export Detail
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

    [DisplayName("Goods ID")]
    [MaxLength(50)]
    public string SKUId { get; set; }
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
    [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
    [Range(0.01, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal UnitPrice { get; set; }
    [DisplayName("Total Price")]
    [Required(ErrorMessage = "Total Price is Required")]
    [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal TotalPrice { get; set; }
    #endregion

    #region Sale Export Document
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

    #region Sale Export Payment
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

    #region Vehicle Information
    [DisplayName("Driver Name")]
    [MaxLength(50)]
    public string DriverName { get; set; }
    [DisplayName("Driver Mobile")]
    // [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string DriverMobile { get; set; }
    [DisplayName("Is Require Registration")]
    public bool IsRequireRegistration { get; set; }
    [DisplayName("Vehicle Type")]
    public int? VehicleTypeId { get; set; }
    [DisplayName("Vehicle Registration No")]
    [MaxLength(50)]
    public string VehicleRegistrationNo { get; set; }
    #endregion

    public IEnumerable<VmSaleExportDetail> VmSaleExportDetails { get; set; }
    public IEnumerable<VmSaleLocalDocument> VmSaleLocalDocuments { get; set; }
    public IEnumerable<VmSaleLocalPayment> VmSaleLocalPayments { get; set; }
    public IEnumerable<Customer> CustomerList { get; set; }
    public IEnumerable<CustomSelectListItem> DeliveryMethodSelectList { get; set; }

    public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; }
    public IEnumerable<CustomSelectListItem> CountrySelectList { get; set; }
    public IEnumerable<CustomSelectListItem> ExportTypeSelectList { get; set; }
    public IEnumerable<SpGetProductForSale> ProductForSaleList { get; set; }
    public IEnumerable<CustomSelectListItem> MeasurementUnitSelectList { get; set; }
    public IEnumerable<CustomSelectListItem> DocumentTypeSelectList { get; set; }
    public IEnumerable<PaymentMethod> PaymentMethodList { get; set; }

    public IEnumerable<CustomSelectListItem> BankSelectList { get; set; }
    public IEnumerable<VehicleType> VehicleTypesList { get; set; }
}

public class VmSaleExportPost
{
    public VmSaleExportPost()
    {

        VmSaleExportDetails = new List<VmSaleExportDetail>();
        VmSaleLocalDocuments = new List<VmSaleExportDocument>();
        VmSaleLocalPayments = new List<VmSaleExportPayment>();
    }

    public bool? IsTaxInvoicePrined { get; set; }
    public int OrganizationId { get; set; }
    [DisplayName("Branch")]
    [Required]
    public int OrgBranchId { get; set; }
    public int CreatedBy { get; set; }
    public int SalesTypeId { get; set; }
    public DateTime SalesDate { get; set; }

    public int? ExportTypeId { get; set; }
    public int SalesId { get; set; }
    //public decimal? Vdsamount { get; set; }
    [Required(ErrorMessage = "Customer Name is Required")]
    [DisplayName("Customer Name")]
    public int? CustomerId { get; set; }
    // [Required(ErrorMessage = "Customer Mobile is Required")]
    // [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    [DisplayName("Customer Contact No")]
    public string CustomerPhoneNumber { get; set; }
    [Required(ErrorMessage = "Order No is Required")]
    [DisplayName("Order No")]
    public string WorkOrderNo { get; set; }
    [Required(ErrorMessage = "Delivery Typ. is Required")]
    [DisplayName("Delivery Typ.")]
    public int SalesDeliveryTypeId { get; set; }
    [Required(ErrorMessage = "Delivery Date is Required")]
    [DisplayName("Delivery Date")]
    public DateTime? DeliveryDate { get; set; }
    // [Required(ErrorMessage = "Rec. Name is Required")]
    [DisplayName("Rec. Name")]
    public string ReceiverName { get; set; }
    // [Required(ErrorMessage = "Rec. Contact No. is Required")]
    [DisplayName("Rec. Contact No.")]
    // [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string ReceiverContactNo { get; set; }
    [Required(ErrorMessage = "Delivery Address is Required")]
    [DisplayName("Delivery Address")]
    public string ShippingAddress { get; set; }
    [Required(ErrorMessage = "Delivery Country is Required")]
    [DisplayName("Delivery Country")]
    public int? ShippingCountryId { get; set; }
    [Required(ErrorMessage = "PO No. is Required")]
    [DisplayName("PO No.")]
    [MaxLength(35)]
    public string PONo { get; set; }
    [Required(ErrorMessage = "LC No. is Required")]
    [DisplayName("LC No.")]
    public string LcNo { get; set; }
    // [Required(ErrorMessage = "LC Date is Required")]
    [DisplayName("LC Date")]
    public DateTime? LcDate { get; set; }
    [Required(ErrorMessage = "Com. Inv. No.")]
    [DisplayName("Com. Inv. No.")]
    public DateTime? BillOfEntry { get; set; }
    [Required(ErrorMessage = "Com. Date is Required")]
    [DisplayName("Com. Date")]
    public DateTime? BillOfEntryDate { get; set; }
    // [Required(ErrorMessage = "LC Terms is Required")]
    [DisplayName("LC Terms")]
    public string TermsOfLc { get; set; }
    // [Required(ErrorMessage = "Due Date is Required")]
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
    // [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string DriverMobile { get; set; }
    [DisplayName("Is Require Registration")]
    public Boolean IsRequireRegistration { get; set; }
    [DisplayName("Vehicle Type")]
    public int? VehicleTypeId { get; set; }
    [DisplayName("Vehicle Registration No")]
    [MaxLength(50)]
    public string VehicleRegistrationNo { get; set; }
    #endregion

    public IEnumerable<VmSaleExportDetail> VmSaleExportDetails { get; set; }
    public IEnumerable<VmSaleExportDocument> VmSaleLocalDocuments { get; set; }
    public IEnumerable<VmSaleExportPayment> VmSaleLocalPayments { get; set; }

}
public class VmSaleExportDetail
{
    public int SalesDetailId { get; set; }
    public int SalesId { get; set; }
    [DisplayName("VAT Type")]
    public int ProductVatTypeId => 1;
    [Required(ErrorMessage = "Product is Required")]
    [DisplayName("Product")]
    public int ProductId { get; set; }

    [DisplayName("HS Code")]
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

    [DisplayName("GOODS ID")]
    [MaxLength(50)]
    public string SKUId { get; set; }
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
public class VmSaleExportDocument
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
public class VmSaleExportPayment
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