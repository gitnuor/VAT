using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels;

public class VmCalculationBookParam
{
    public VmCalculationBookParam()
    {
        PriceSetupList = new List<SelectListItem>();
        PurchaseCalcBook = new List<SpPurchaseCalcBook>();
        PurchaseSaleCalcBook = new List<SpPurchaseSaleCalcBook>();
        SaleCalcBook = new List<SpSalesCalcBook>();
    }

    public IEnumerable<SelectListItem> PriceSetupList { get; set; }
    [Required(ErrorMessage ="Month is Required")]
    public int Month { get; set; }
    [Required(ErrorMessage = "Year is Required")]
    [Range(minimum:2021,maximum:2050,ErrorMessage ="Value Must be Between 2021 and 2050")]
    public int Year { get; set; }
    [Required(ErrorMessage = "Product is Required")]
    [DisplayName("Product")]
    public int ProductId { get; set; }
    public int VendorId { get; set; }
    public int OgrId { get; set; }
	[DisplayName("Branch")]
    public int? OgrBranchId { get; set; }
    public int CustomerId { get; set; }
    public string OrgName { get; set; }
    public string OrgBin { get; set; }
    public string OrgAddress { get; set; }

    [DisplayName("From Date")]
    public DateTime? FromDate { get; set; }

	[DisplayName("To Date")]
    public DateTime? ToDate { get; set; }

    public string ReportUrl { get; set; }
    public string FromDateToDisplay { get; set; }
    public string ToDateToDisplay { get; set; }
    [Required(ErrorMessage ="Language is required")]
    [DisplayName("Language")]
    public int Language { get; set; }
    public List<SpPurchaseCalcBook> PurchaseCalcBook { get; set; }
    public List<SpPurchaseSaleCalcBook> PurchaseSaleCalcBook { get; set; }
    public List<SpSalesCalcBook> SaleCalcBook { get; set; }
    public List<SelectListItem> LanguageList { get; set; }
    public IEnumerable<CustomSelectListItem> BranchList { get; set; }
	[DisplayName("Report Option")]
	[Range(1, 4, ErrorMessage = "Please select an option")]
	public int ReportProcessOptionId { get; set; }

	public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
}