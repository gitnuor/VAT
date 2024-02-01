using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels;

public class VmPriceSetupReportParam
{
    public VmPriceSetupReportParam()
    {
        PriceSetupList = new List<SelectListItem>();
        PriceDeclarationList = new List<Sp4p3>();
        LanguageList = new List<SelectListItem>();
    }
    [DisplayName("Product")]
    public int PriceDeclarId { get; set; }
    public int OrgId { get; set; }

    public string ReportUrl { get; set; }
    public IEnumerable<SelectListItem> PriceSetupList { get; set; }
    public List<Sp4p3> PriceDeclarationList { get; set; }
    public int Language { get; set; }
    public List<SelectListItem> LanguageList { get; set; }

    public string OrgAddress { get; set; }
    public string OrgName { get; set; }
    public string OrgBin { get; set; }


}