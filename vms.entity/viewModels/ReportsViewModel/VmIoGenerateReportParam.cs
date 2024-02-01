using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmIoGenerateReportParam
{
    public VmIoGenerateReportParam()
    {
        PriceSetupList = new List<SelectListItem>();
        PriceDeclarationList = new List<Sp4p3>();
    }

    [DisplayName("Product")]
    public int ProductId { get; set; }
    public IEnumerable<SelectListItem> PriceSetupList { get; set; }
    public List<Sp4p3> PriceDeclarationList { get; set; }


}