using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class VmConvertToVds
{
	[Display(Name = "Sales Id")]
    public int SalesId { get; set; }
    [Display(Name = "Sales Id")]
	public string EncryptedId { get; set; }
    [Display(Name = "Customer")]
    public string CustomerName { get; set; }
	[Display(Name = "Invoice No.")]
    public string InvoiceNo { get; set; }
    [Display(Name = "Challan No.")]
    public string VatChallanNo { get; set; }
    [Display(Name = "Challan Time")]
    public string VatChallanTime { get; set; }
	[Display(Name = "Total Price Without VAT")]
    public decimal TotalPriceWithoutVat { get; set; }
	[Display(Name = "Total VAT")]
    public decimal TotalVat { get; set; }
	[Display(Name = "VDS Amount")]
    public decimal Vdsamount { get; set; }
	[Display(Name = "Sales Date")]
    public DateTime SalesDate { get; set; }
	[Display(Name = "VDS Date")]
    public DateTime VdsDate { get; set; }

}