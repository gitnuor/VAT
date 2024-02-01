using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels.ReportsViewModel;

public class VmPurchaseReportByProduct
{
    public VmPurchaseReportByProduct()
    {
        ProductList = new List<CustomSelectListItem>();
        PurchaseList = new List<SpGetPurchaseReportByProduct>();
    }
    [DisplayName("From Date")]
    [DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
    [DateLessThan(nameof(ToDate), AllowEquality = true, ErrorMessage = "From Date must be less than or equal to To Date")]
    [Required(ErrorMessage = "From Date is required")]
    public DateTime FromDate { get; set; }
    [DisplayName("To Date")]
    [DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
    [Required(ErrorMessage = "To Date is required")]
    public DateTime ToDate { get; set; }
    [Required(ErrorMessage ="Product is required")]
    [DisplayName("Product")]
    public int ProductId { get; set; }        
    public int Reason { get; set; }
    public string OrgName { get; set; }
    public string OrgAddress { get; set; }
    public List<SpGetPurchaseReportByProduct> PurchaseList { get; set; }
    public IEnumerable<CustomSelectListItem> ProductList { get; set; }

    [DisplayName("Report Option")]
    [Range(1, 4, ErrorMessage = "Please select an option")]
    public int ReportProcessOptionId { get; set; }

    public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
}