using System.Collections.Generic;
using vms.entity.StoredProcedureModel.MushakReturn;

namespace vms.entity.viewModels.Payment;

public class TdsPaymentChallanViewModel
{
	public int TdsPaymentId { get; set; }
	public List<SpGetTdsPaymentChallan> ChallanList { get; set; }
}