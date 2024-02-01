using System;
using System.Collections.Generic;

namespace vms.entity.viewModels;

public class VmSalesCreditNotePost
{
	public VmSalesCreditNotePost()
	{
		vmSalesCreditNoteDetials = new List<VmSalesCreditNoteDetial>();
	}

	public int? CreatedBy { get; set; }
	public DateTime? CreatedTime { get; set; }
	public int SalesId { get; set; }
	public string ReasonOfReturn { get; set; }
	public DateTime ReturnDate { get; set; }
	public int VehicleTypeId { get; set; }
	public string VehicleName { get; set; }
	public string VehicleRegNo { get; set; }
	public string VehicleDriverName { get; set; }
	public string VehicleDriverContactNo { get; set; }
	public string VoucherNo { get; set; }
    public int? OrgBranchId { get; set; }

    public List<VmSalesCreditNoteDetial> vmSalesCreditNoteDetials { get; set; }

}