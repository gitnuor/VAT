using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class SalesApproveOrRejectDisplayViewModel
{
	public Sale Sale { get; set; }
	public SaleApproveOrRejectViewModel ApproveOrRejectViewModel { get; set; }
	public IEnumerable<SalesDetail> SalesDetails { get; set; }

}