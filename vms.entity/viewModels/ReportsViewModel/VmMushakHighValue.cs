using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HighValue;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmMushakHighValue
{

    public IEnumerable<SpPurcSalesChallanForHighValuePurchase> SpPurcSalesChallanForHighValuePurchaseList { get; set; }
    public SpPurcSalesChallanForHighValuePurchase SpPurcSalesChallanForHighValuePurchase { get; set; }
    public IEnumerable<SpPurcSalesChallanForHighValueSale> SpPurcSalesChallanForHighValueSaleList { get; set; }
}