using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.SalesLocal;

public class SalesCombinedPostDto
{
	[Required] public string SalesId { get; set; }
	[DisplayName("Branch")] [Required] public string BranchId { get; set; }
	public DateTime SalesDate { get; set; }
	public string InvoiceNo { get; set; }
	public DateTime? InvoiceDate { get; set; }
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

	[Required(ErrorMessage = "Rec. Name is Required")]
	[DisplayName("Rec. Name")]
	public string ReceiverName { get; set; }

	[DisplayName("Rec. Contact No.")]
	public string ReceiverContactNo { get; set; }

	[Required(ErrorMessage = "Rec. Address is Required")]
	[DisplayName("Rec. Address")]
	public string ShippingAddress { get; set; }

	[Required(ErrorMessage = "Sales Type is Required")]
	[DisplayName("Sales Type")]
	public int SalesTypeId { get; set; }

	[DisplayName("LC No")]
	[MaxLength(50)]
	public string LcNo { get; set; }

	[DisplayName("LC Date")]
	public DateTime? LcDate { get; set; }

	[DisplayName("PO No.")]
	public string PONo { get; set; }

	[DisplayName("Remarks")]
	[MaxLength(500)]
	public string SalesRemarks { get; set; }
	[DisplayName("Created By")]
	[MaxLength(100)]
	[Required]
	public string CreatedBy { get; set; }

	#region Vehicle Information

	[DisplayName("Driver Name")] public string DriverName { get; set; }
	[DisplayName("Driver Mobile")] public string DriverMobile { get; set; }

	[DisplayName("Is Require Registration")]
	public bool IsRequireRegistration { get; set; }

	[DisplayName("Vehicle Name")] public string VehicleName { get; set; }

	[DisplayName("Vehicle Registration No")]
	public string VehicleRegistrationNo { get; set; }

	#endregion


	public IEnumerable<SalesCombinedDetailPostWithSalesDto> Details { get; set; } =
		new List<SalesCombinedDetailPostWithSalesDto>();
}