using System.Collections.Generic;

namespace vms.entity.viewModels;

public class DataimportSalesFinal
{
    //        public List<DatauploadSales> Sales { get; set; }
    //        public List<DatauploadSaleDetails> SalesDetails { get; set; }
    //        public List<DatauploadPaymentSale> Payments { get; set; }
    public List<vmSalesPost> Sales { get; set; }

    public List<vmSalesDetailPost> SalesDetails { get; set; }
    public List<vmSalesPaymentPost> Payments { get; set; }
}