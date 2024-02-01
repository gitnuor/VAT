using System;
using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmPurchaseSalesCalcBookNew
{
    public int CustomerId { get; set; }
        
    public int ogrId { get; set; }
    public int productId { get; set; }
    public int? PriceDeclarId { get; set; }
    public DateTime fromDate { get; set; }
    public DateTime toDate { get; set; }
    public string Product { get; set; }
    public List<SpPurchaseSaleCalcBook> PurSaleCallBook { get; set; }
}