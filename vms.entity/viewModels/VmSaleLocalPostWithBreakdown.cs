using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace vms.entity.viewModels;

public class VmSaleLocalPostWithBreakdown
{
    public VmSaleLocalPostWithBreakdown()
    {
        Products = new List<VmSaleLocalDetail>();
        Documents = new List<VmSaleLocalDocument>();
        Payments = new List<VmSaleLocalPayment>();
        SaleBreakDowns = new List<VmSaleLocalBreakDown>();
    }

    public bool? IsTaxInvoicePrined { get; set; }
    public int OrganizationId { get; set; }
    [DisplayName("Branch")]
    [Required]
    public int OrgBranchId { get; set; }
    public int CreatedBy { get; set; }
    public int SalesTypeId { get; set; }
    public DateTime SalesDate { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public int SalesId { get; set; }
    [DisplayName("Is VDS?")]
    public bool IsVatDeductedInSource { get; set; }
    [DisplayName("VDS Amount")]
    public decimal? VdsAmount { get; set; }
    [Required(ErrorMessage = "Cus. Name is Required")]
    [DisplayName("Cus. Name")]
    public int? CustomerId { get; set; }
    [Required(ErrorMessage = "Cus. Mobile is Required")]
    [DisplayName("Cus. Mobile")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
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
    [Required(ErrorMessage = "Rec. Address is Required")]
    [DisplayName("Rec. Address")]
    public string ShippingAddress { get; set; }
    [Required(ErrorMessage = "PO No. is Required")]
    [DisplayName("PO No.")]
    public string PONo { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(500)]
    public string SalesRemarks { get; set; }
    [DisplayName("Sales Breakdown")]
    [NotMapped, Required(ErrorMessage = "File is Required")]
    public IFormFile FileSalesBreakDown { get; set; }

	#region Vehicle Information
	[DisplayName("Driver Name")]
    public string DriverName { get; set; }
    [DisplayName("Driver Mobile")]
    public string DriverMobile { get; set; }
    [DisplayName("Is Require Registration")]
    public Boolean IsRequireRegistration { get; set; }
    [DisplayName("Vehicle Type")]
    public int? VehicleTypeId { get; set; }
    [DisplayName("Vehicle Registration No")]
    public string VehicleRegistrationNo { get; set; }
    #endregion


    public IEnumerable<VmSaleLocalDetail> Products { get; set; }
    public IEnumerable<VmSaleLocalDocument>   Documents{ get; set; }
    public IEnumerable<VmSaleLocalPayment> Payments { get; set; }
    public IEnumerable<VmSaleLocalBreakDown> SaleBreakDowns { get; set; }
}

//public class VmSaleLocalBreakDownWith
//{
//	public string Id { get; set; }
//	public string Description { get; set; }
//	public decimal Amount { get; set; }
//}