using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmInputOutputCoEfficientReport
{
    public VmInputOutputCoEfficientReport()
    {
        ProductList = new List<CustomSelectListItem>();
        PriceSetups = new List<PriceSetup>();
    }
    [Required(ErrorMessage ="Product is required")]
    [DisplayName("Product")]
    public int ProductId { get; set; }        
    public int Reason { get; set; }
    public string OrgName { get; set; }
    public string OrgAddress { get; set; }
    public List<PriceSetup> PriceSetups { get; set; }
    public IEnumerable<CustomSelectListItem> ProductList { get; set; }
    [DisplayName("Report Option")]
    [Range(1, 4, ErrorMessage = "Please select an option")]
    public int ReportProcessOptionId { get; set; }
    public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }

}