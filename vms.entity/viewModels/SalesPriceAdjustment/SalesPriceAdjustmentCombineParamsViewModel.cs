using System;
using System.Collections.Generic;
using vms.entity.viewModels.VmSalesCombineParamsModels;

namespace vms.entity.viewModels.SalesPriceAdjustment;

public class SalesPriceAdjustmentCombineParamsViewModel
{
	public SalesPriceAdjustmentCombineParamsViewModel()
	{
		DetailList = new List<SalesPriceAdjustmentCombineDetailParamsViewModel>();
		ContentInfoJson = new List<SalesPriceAdjustmentCombineContentParamsViewModel>();
	}
    public List<SalesPriceAdjustmentCombineDetailParamsViewModel> DetailList { get; set; }
    public List<SalesPriceAdjustmentCombineContentParamsViewModel> ContentInfoJson { get; set; }
	public int SalesPriceAdjustmentId { get; set; }

	public int AdjustmentTypeId { get; set; }

	public int OrganizationId { get; set; }

	public int? OrgBranchId { get; set; }

	public int SalesId { get; set; }

	public string InvoiceNo { get; set; }

	public DateTime? InvoiceDate { get; set; }

	public string ClientNoteNo { get; set; }

	public DateTime? ClientNoteTime { get; set; }

	public string ReasonOfChange { get; set; }

	public int? VehicleTypeId { get; set; }

	public string VehicleName { get; set; }

	public string VehicleRegNo { get; set; }

	public string VehicleDriverName { get; set; }

	public string VehicleDriverContactNo { get; set; }

	public bool IsNotePrinted { get; set; }

	public string NoteNo { get; set; }

	public int? NotePrintedBy { get; set; }

	public DateTime? NotePrintedTime { get; set; }

	public string AdjustmentRemarks { get; set; }

	public string ReferenceKey { get; set; }

	public int? CreatedBy { get; set; }

	public DateTime? CreatedTime { get; set; }
}