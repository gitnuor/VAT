using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.SalesDeemedExport;

public class SalesDeemedExportPostDto
{
	[DisplayName("Sales Id")]
	[Required(ErrorMessage = "{0} is Required")]
	public string SalesId { get; set; }

	[DisplayName("Branch")]
	[Required(ErrorMessage = "{0} is Required")]
	public string BranchId { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Sales Date")]
	public DateTime SalesDate { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Customer")]
	public string CustomerId { get; set; }

	[DisplayName("Work Order No")] public string WorkOrderNo { get; set; }

	[DisplayName("PO No.")]
	[MaxLength(50)]
	public string PONo { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Delivery Type")]
	public string SalesDeliveryTypeId { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Delivery Date")]
	public DateTime DeliveryDate { get; set; }

	[Required(ErrorMessage = "Rec. Name is Required")]
	[DisplayName("Rec. Name")]
	public string ReceiverName { get; set; }

	[DisplayName("Rec. Contact No.")]
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
	public string ReceiverContactNo { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Shipping Address")]
	public string ShippingAddress { get; set; }

	[DisplayName("LC No.")] public string LcNo { get; set; }

	[DisplayName("LC Date")] public DateTime LcDate { get; set; }

	[DisplayName("LC Terms")] public string TermsOfLc { get; set; }

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

	[DisplayName("Vehicle Name")] public string VehicleName { get; set; }

	[DisplayName("Vehicle Registration No")]
	[MaxLength(50)]
	public string VehicleRegistrationNo { get; set; }

	#endregion

	public IEnumerable<SalesDeemedExportDetailPostWithSalesDto> Details { get; set; } =
		new List<SalesDeemedExportDetailPostWithSalesDto>();

	public IEnumerable<DocumentPostWithObjectDto> Documents { get; set; } = new List<DocumentPostWithObjectDto>();
}