﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.BranchTransferSend;

public class BranchTransferSendParamViewModel
{
	public BranchTransferSendParamViewModel()
	{
		Products = new List<BranchTransferSendProductParamViewModel>();
		Documents = new List<BranchTransferSendContentParamsViewModel>();
	}

	#region Vehicle Information

	[DisplayName("Driver Name")]
	[MaxLength(50)]
	public string DriverName { get; set; }

	[DisplayName("Driver Mobile")]
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
	public string DriverMobile { get; set; }

	[DisplayName("Is Require Registration")]
	public Boolean IsRequireRegistration { get; set; }

	[DisplayName("Vehicle Type")] public int VehicleTypeId { get; set; }
	public string VehicleName { get; set; }

	[DisplayName("Vehicle Registration No")]
	[MaxLength(50)]
	public string VehicleRegistrationNo { get; set; }

	#endregion
	public int OrganizationId { get; set; }

	public int BranchTransferSendId { get; set; }

	[DisplayName("Sender Branch")]
	[Required(ErrorMessage = "{0} is Required")]
	public int SenderBranchId { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Receiver Branch")]
	public int ReceiverBranchId { get; set; }

	[MaxLength(50, ErrorMessage = "Max Length 50 allowed")]
	[DisplayName("Invoice No.")]
	public string InvoiceNo { get; set; }

	[DisplayName("Invoice Date")] public DateTime? InvoiceDate { get; set; }

	[Required(ErrorMessage = "Del. Date is Required")]
	[DisplayName("Del. Date")]
	public DateTime? DeliveryDate { get; set; }
	public DateTime? BranchTransferDate { get; set; }

	[DisplayName("Rec. Name")]
	[MaxLength(50)]
	public string ReceiverName { get; set; }

	[DisplayName("Rec. Contact No.")]
	[RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
	public string ReceiverContactNo { get; set; }

	[Required(ErrorMessage = "Del. Address is Required")]
	[DisplayName("Del. Address")]
	[MaxLength(100)]
	public string ShippingAddress { get; set; }
	public int? ShippingCountryId { get; set; } = null;

	[DisplayName("Remarks")]
	[MaxLength(500)]
	public string BranchTransferSendRemarks { get; set; }
	public bool IsComplete { get; set; }
	public bool IsPosted { get; set; }
	public int? PostedBy { get; set; } = null;
	public DateTime? PostedTime { get; set; } = null;

	public bool IsTransferChallanPrined { get; set; } = false;
	public string TransferChallanNo { get; set; } = null;
	public DateTime? TransferChallanPrintedTime { get; set; } = null;
	public int? TransferChallanPrintedBy { get; set; } = null;
	public string ReferenceKey { get; set; } = null;
	public int? CreatedBy { get; set; } = null;
	public DateTime? CreatedTime { get; set; } = null;


	public IEnumerable<BranchTransferSendProductParamViewModel> Products { get; set; }
	public IEnumerable<BranchTransferSendContentParamsViewModel> Documents { get; set; }
}