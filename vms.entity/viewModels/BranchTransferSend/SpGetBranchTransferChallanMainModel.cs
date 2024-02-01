using System;

namespace vms.entity.viewModels.BranchTransferSend;

public class SpGetBranchTransferChallanMainModel
{
	public int BranchTransferSendId { get; set; }
	public int OrganizationId { get; set; }
	public string OrganizationName { get; set; }
	public string OrganizationAddress { get; set; }
	public string OrganizationBinNo { get; set; }
	public int OrgBranchSenderId { get; set; }
	public string SenderBranchName { get; set; }
	public string SenderBranchAddress { get; set; }
	public int OrgBranchReceiverId { get; set; }
	public string ReceiverBranchName { get; set; }
	public string ReceiverBranchAddress { get; set; }
	public int BranchTransferSendSlNo { get; set; }
	public string InvoiceNo { get; set; }
	public DateTime? InvoiceDate { get; set; }
	public string TransferChallanNo { get; set; }
	public DateTime? TransferChallanPrintedTime { get; set; }
	public int? TransferChallanPrintedBy { get; set; }
	public string ChallanGeneratorName { get; set; }
	public string ChallanGeneratorMobile { get; set; }
	public string ChallanGeneratorDesignation { get; set; }
	public string VatResponsiblePersonName { get; set; }
	public string VatResponsiblePersonDesignation { get; set; }
	public string VatResponsiblePersonSignUrl { get; set; }
	public string InvoiceStatus { get; set; }
	public bool IsTransferChallanPrinted { get; set; }
	public string ReceiverName { get; set; }
	public string ReceiverContactNo { get; set; }
	public string ShippingAddress { get; set; }
	public DateTime BranchTransferSendDate { get; set; }
	public DateTime? DeliveryDate { get; set; }
	public int VehicleTypeId { get; set; }
	public string VehicleTypeName { get; set; }
	public string VehicleName { get; set; }
	public string VehicleDriverName { get; set; }
	public string VehicleDriverContactNo { get; set; }
	public string VehicleRegNo { get; set; }
	public string VehicleTypeAndRegistrationNo { get; set; }
	public string BranchTransferSendRemarks { get; set; }
	public bool IsComplete { get; set; }
}