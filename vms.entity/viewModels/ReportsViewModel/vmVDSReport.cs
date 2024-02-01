using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HTMLMushak;
namespace vms.entity.viewModels.ReportsViewModel;

public class vmVDSReport
{

    public int PurchaseId { get; set; }
    public string InvoiceNo { get; set; }
    public List<Sp6p6> Vds { get; set; }
}