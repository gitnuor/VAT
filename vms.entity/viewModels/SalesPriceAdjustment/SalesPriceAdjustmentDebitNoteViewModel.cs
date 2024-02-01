using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.SalesPriceAdjustment;

public class SalesPriceAdjustmentDebitNoteViewModel
{
    public SalesPriceAdjustmentDebitNoteViewModel()
    {
        SalesDetails = new List<SalesDetail>();
    }
    public Sale Sale { get; set; }
    public IEnumerable<SalesDetail> SalesDetails { get; set; }
    //Start Main Form Field
    public int SalesId { get; set; }
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
    public string VehicleDriverName { get; set; }
    public string VehicleDriverContactNo { get; set; }
    [DisplayName("Voucher No.")]
    [Required(ErrorMessage = "Voucher No is Required")]
    [MaxLength(30)]
    public string VoucherNo { get; set; }
    //End Main Form Field

    //Details Field
    public int SalesDetailId { get; set; }
    [DisplayName("Product")]
    [Required(ErrorMessage = "Product is Required")]
    public int ProductId { get; set; }
    [DisplayName("Change Quantity")]
    [Required(ErrorMessage = "Change Quantity is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal QuantityToChange { get; set; }
    [DisplayName("Change Amount")]
    public decimal ChangeAmount { get; set; }
    [DisplayName("Change per Item")]
	public decimal ChangeAmountPerItem { get; set; }
    [DisplayName("Change VAT (%)")]
    public decimal ChangeVatParcent { get; set; }

    [DisplayName("Change VAT")]
    public decimal ChangeVat { get; set; }

    [DisplayName("Change SD (%)")]
    public decimal ChangeSdParcent { get; set; }

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
    public decimal VatParcent { get; set; }

    [DisplayName("VAT")]
    public decimal Vat { get; set; }

    [DisplayName("SD (%)")]
    public decimal SdParcent { get; set; }

    [DisplayName("SD")]
    public decimal Sd { get; set; }
    [MaxLength(150)]
    public string ReasonOfChangeInDetail { get; set; }
    //Detail Field
}