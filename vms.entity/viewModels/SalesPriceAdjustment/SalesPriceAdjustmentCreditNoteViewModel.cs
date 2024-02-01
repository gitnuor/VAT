using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.SalesPriceAdjustment;

public class SalesPriceAdjustmentCreditNoteViewModel
{
    public SalesPriceAdjustmentCreditNoteViewModel()
    {
        SalesDetails = new List<SalesDetail>();
		//OrgBranchList = new List<OrgBranch>();
        OrgBranchList = new List<CustomSelectListItem>();
        VehicleTypeList = new List<CustomSelectListItem>();
    }
    public Sale Sale { get; set; }
    public IEnumerable<SalesDetail> SalesDetails { get; set; }
	[DisplayName("Branch")]
	[Required]
	public int OrgBranchId { get; set; }
    //public IEnumerable<OrgBranch> OrgBranchList { get; set; }
    public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; }
    public IEnumerable<CustomSelectListItem> VehicleTypeList { get; set; }
    //Start Main Form Field
    public int SalesId { get; set; }
	[DisplayName("Invoice No")]
	public string InvoiceNo { get; set; }
	[DisplayName("Invoice Date")]
	public DateTime? InvoiceDate { get; set; }
	[DisplayName("Client Note No")]
	public string ClientNoteNo { get; set; }
	[DisplayName("Client Note Time")]
	public DateTime? ClientNoteTime { get; set; }
	[DisplayName("Reason Of Change")]
    [Required(ErrorMessage = "Reason Of Change is Required")]
    [MaxLength(150)]
    public string ReasonOfChange { get; set; }
    [Required(ErrorMessage = "Change Date is Required")]
    [DisplayName("Change Date")]
    public DateTime? ChangeDate { get; set; }
    [DisplayName("Vehicle Type")]
    [Required(ErrorMessage = "Vehicle Type is Required")]
    public int VehicleTypeId { get; set; }
    public string VehicleName { get; set; }
    [DisplayName("Vehicle Reg. No.")]
    [MaxLength(30)]
    public string VehicleRegNo { get; set; }
	[DisplayName("Vehicle Driver Name")]
	[MaxLength(100)]
    public string VehicleDriverName { get; set; }
	[DisplayName("Vehicle Driver Contact No.")]
	[MaxLength(20)]
    public string VehicleDriverContactNo { get; set; }
    //End Main Form Field

    //Details Field
    [DisplayName("Product")]
    public int SalesDetailId { get; set; }
    [DisplayName("Change Quantity")]
    [Required(ErrorMessage = "Change Quantity is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal QuantityToChange { get; set; }
    [DisplayName("Change Amount")]
    public decimal ChangeAmount { get; set; }
    [DisplayName("Change Unit Price")]
    public decimal ChangeAmountPerItem { get; set; }
    [DisplayName("Change VAT (%)")]
    public decimal ChangeVatPercent { get; set; }

    [DisplayName("Change VAT")]
    public decimal ChangeVat { get; set; }

    [DisplayName("Change SD (%)")]
    public decimal ChangeSdPercent { get; set; }

    [DisplayName("Change SD")]
    public decimal ChangeSd { get; set; }
    public decimal Quantity { get; set; }
    public int MeasurementUnitId { get; set; }
    [DisplayName("M. Unit")]
    public string MeasurementUnitName { get; set; }
    [DisplayName("Amount")]
    public decimal Amount { get; set; }
    [DisplayName("Unit Price")]
    public decimal UnitPrice { get; set; }
    [DisplayName("VAT (%)")]
    public decimal VatPercent { get; set; }

    [DisplayName("VAT")]
    public decimal Vat { get; set; }

    [DisplayName("SD (%)")]
    public decimal SdPercent { get; set; }

    [DisplayName("SD")]
    public decimal Sd { get; set; }
    [MaxLength(150)]
    [Display(Name = "Reason of Change")]
    [Required]
    public string ReasonOfChangeInDetail { get; set; }
	//Detail Field


	public IEnumerable<VehicleType> VehicleTypesList { get; set; }
}