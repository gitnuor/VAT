using System.Collections.Generic;
using vms.entity.StoredProcedureModel.MushakReturn;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmMushakReturnPaymentChallan
{
    public int MushakPaymentId { get; set; }
    public List<SpGetMushakReturnPaymentChallan> ChallanList { get; set; }
}