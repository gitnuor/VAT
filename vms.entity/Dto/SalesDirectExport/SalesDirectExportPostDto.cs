using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace vms.entity.Dto.SalesDirectExport;

public class SalesDirectExportPostDto
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

	[Required(ErrorMessage = "Delivery Address is Required")]
	[DisplayName("Delivery Address")]
	public string ShippingAddress { get; set; }

	[Required(ErrorMessage = "LC No. is Required")]
	[DisplayName("LC No.")]
	public string LcNo { get; set; }

	[Required(ErrorMessage = "LC Date is Required")]
	[DisplayName("LC Date")]
	public DateTime? LcDate { get; set; }

	[DisplayName("LC Terms")] public string TermsOfLc { get; set; }

	[DisplayName("Remarks")]
	[MaxLength(500)]
	public string SalesRemarks { get; set; }

	#region Vehicle Information

	[DisplayName("Driver Name")]
	[MaxLength(50)]
	public string DriverName { get; set; }

	[DisplayName("Driver Mobile")] public string DriverMobile { get; set; }
	[DisplayName("Vehicle Name")] public string VehicleName { get; set; }

	[DisplayName("Vehicle Registration No")]
	[MaxLength(50)]
	public string VehicleRegistrationNo { get; set; }

	#endregion

	public IEnumerable<SalesDirectExportDetailPostWithSalesDto> Details { get; set; } =
		new List<SalesDirectExportDetailPostWithSalesDto>();

	public IEnumerable<DocumentPostWithObjectDto> Documents { get; set; } = new List<DocumentPostWithObjectDto>();
}