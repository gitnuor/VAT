using System.Collections.Generic;

namespace vms.entity.StoredProcedureModel;

public class SpGetDashBoardInfo
{
    public SpGetDashBoardInfo()
    {
        DailyPurchases = new List<SpGetDashBoardInfoDailyPurchase>();
    }
    public SpGetDashBoardInfoSummery Summery { get; set; }
    public IEnumerable<SpGetDashBoardInfoDailyPurchase> DailyPurchases { get; set; }
}