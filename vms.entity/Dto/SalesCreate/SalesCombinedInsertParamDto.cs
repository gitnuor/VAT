using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.SalesCreate;

public class SalesCombinedInsertParamDto
{
    [DisplayName("Token")] [Required] public string Token { get; set; }
    [DisplayName("Token")] [Required] public string SalesId { get; set; }
    [DisplayName("Branch")] [Required] public string BranchId { get; set; }
    public DateTime SalesDate { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }

    public string VatChallanNo { get; set; }

    public int OrganizationId { get; set; }

    public int OrgBranchId { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }

    [DisplayName("Is VDS?")] public bool IsVatDeductedInSource { get; set; }

    [DisplayName("VDS Amount")] public decimal? VdsAmount { get; set; }

    [Required(ErrorMessage = "{0} is Required")]
    [DisplayName("Customer")]
    public string CustomerId { get; set; }

    [DisplayName("Customer Name")] public string CustomerName { get; set; }

    [DisplayName("Customer BIN")] public string CustomerBin { get; set; }

    [DisplayName("Customer NID")] public string CustomerNid { get; set; }

    [DisplayName("Customer Address")] public string CustomerAddress { get; set; }

    [DisplayName("Customer Mobile")] public string CustomerPhoneNo { get; set; }

    [DisplayName("Work Order No")] public string WorkOrderNo { get; set; }

    [Required(ErrorMessage = "Sales Delivery Typ. is Required")]
    [DisplayName("Sales Delivery Typ.")]
    public int SalesDeliveryTypeId { get; set; }

    [Required(ErrorMessage = "Delivery Date is Required")]
    [DisplayName("Delivery Date")]
    public DateTime? DeliveryDate { get; set; }

    public int? DeliveryMethodId { get; set; }
    public int? ExportTypeId { get; set; }
    public string LcNo { get; set; }
    public DateTime? LcDate { get; set; }
    public string BillOfEntry { get; set; }
    public DateTime? BillOfEntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string TermsOfLc { get; set; }
    public string CustomerPoNumber { get; set; }
    public bool IsComplete { get; set; }
    public bool? IsTaxInvoicePrinted { get; set; }
    public DateTime? TaxInvoicePrintedTime { get; set; }

    [Required(ErrorMessage = "Rec. Name is Required")]
    [DisplayName("Rec. Name")]
    public string ReceiverName { get; set; }

    [Required(ErrorMessage = "Rec. Contact No. is Required")]
    [DisplayName("Rec. Contact No.")]
    public string ReceiverContactNo { get; set; }

    [Required(ErrorMessage = "Rec. Address is Required")]
    [DisplayName("Rec. Address")]
    public string ShippingAddress { get; set; }

    public int? ShippingCountryId { get; set; }

    public int SalesTypeId { get; set; }

    [Required(ErrorMessage = "PO No. is Required")]
    [DisplayName("PO No.")]
    public string PONo { get; set; }

    [DisplayName("Remarks")]
    [MaxLength(500)]
    public string SalesRemarks { get; set; }

    public string CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }

    #region Vehicle Information

    [DisplayName("Driver Name")] public string DriverName { get; set; }
    [DisplayName("Driver Mobile")] public string DriverMobile { get; set; }

    [DisplayName("Is Require Registration")]
    public bool IsRequireRegistration { get; set; }

    [DisplayName("Vehicle Name")] public string VehicleName { get; set; }

    [DisplayName("Vehicle Registration No")]
    public string VehicleRegistrationNo { get; set; }

    #endregion


    public IEnumerable<SalesDetailCreateParamDto> Details { get; set; } =
        new List<SalesDetailCreateParamDto>();
}