using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels;

public class VmPurchaseDebitNote
{
    public VmPurchaseDebitNote()
    {
        PurchaseDetails = new List<PurchaseDetail>();
        VehicleList = new List<CustomSelectListItem>();
    }
    public Purchase Purchase { get; set; }
    public IEnumerable<PurchaseDetail> PurchaseDetails { get; set; }
    public IEnumerable<CustomSelectListItem> VehicleList { get; set; }
    //Start Main Form Field
    public int PurchaseId { get; set; }
    [DisplayName("Reason Of Return")]
    [Required(ErrorMessage = "Reason Of Return is Required")]
    [MaxLength(150)]
    public string ReasonOfReturn { get; set; }
    [Required(ErrorMessage ="Return Date is Required")]
    [DisplayName("Return Date")]
    public DateTime? ReturnDate { get; set; }
    [DisplayName("Vehicle Type")]
    [Required(ErrorMessage ="Vehicle Type is Required")]
    public int VehicleTypeId { get; set; }
    public string VehicleName { get; set; }
    [DisplayName("Vehicle Reg. No.")]
    [MaxLength(30)]
    public string VehicleRegNo { get; set; }
    public string VehicleDriverName { get; set; }
    public string VehicleDriverContactNo { get; set; }
    [DisplayName("Voucher No")]
    [Required(ErrorMessage = "Voucher No is Required")]
    [MaxLength(30)]
    public string VoucherNo { get; set; }
    //End Main Form Field

    //Details Field
    public int PurchaseDetailId { get; set; }
    [DisplayName("Product")]
    [Required(ErrorMessage = "Product is Required")]
    public int ProductId { get; set; }
    [DisplayName("Return Quantity")]
    [Required(ErrorMessage = "Return Quantity is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal ReturnQuantity { get; set; }
    [DisplayName("Return Amount")]
    public decimal ReturnAmount { get; set; }
    [DisplayName("Return Unit Price")]
    public decimal ReturnUnitPrice { get; set; }
    [DisplayName("Return VAT(%)")]
    public decimal ReturnVatParcent { get; set; }

    [DisplayName("Return VAT")]
    public decimal ReturnVat { get; set; }

    [DisplayName("Return SD(%)")]
    public decimal ReturnSdParcent { get; set; }

    [DisplayName("Return SD")]
    public decimal ReturnSd { get; set; }
    public decimal Quantity { get; set; }
	[DisplayName("M. Unit")]
	public int MeasurementUnitId { get; set; }
    [DisplayName("M. Unit")]
    public string MeasurementUnitName { get; set; }
    [DisplayName("Amount")]
    public decimal Amount { get; set; }
    [DisplayName("Unit Price")]
    public decimal UnitPrice { get; set; }
    [DisplayName("VAT(%)")]
    public decimal VatParcent { get; set; }

    [DisplayName("VAT")]
    public decimal Vat { get; set; }

    [DisplayName("SD (%)")]
    public decimal SdParcent { get; set; }

    [DisplayName("SD")]
    public decimal Sd { get; set; }
    [MaxLength(30)]
    public string ReasonOfReturnInDetail { get; set; }
    //Detail Field
}


public class VmPurchaseDebitNoteDetial
{
    //Details Field
    public int PurchaseDetailId { get; set; }
    [DisplayName("Product")]
    [Required(ErrorMessage = "Product is Required")]
    public int ProductId { get; set; }
    [DisplayName("Return Quantity")]
    [Required(ErrorMessage = "Return Quantity is Required")]
    public decimal ReturnQuantity { get; set; }
    [DisplayName("Return Amount")]
    public decimal ReturnAmount { get; set; }
    [DisplayName("Return U. Price")]
    public decimal ReturnUnitPrice { get; set; }
    [DisplayName("Return VAT (%)")]
    public decimal ReturnVatParcent { get; set; }

    [DisplayName("Return VAT")]
    public decimal ReturnVat { get; set; }

    [DisplayName("Return SD (%)")]
    public decimal ReturnSdParcent { get; set; }

    [DisplayName("Return SD")]
    public decimal ReturnSd { get; set; }
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
    public string ReasonOfReturnInDetail { get; set; }
    //Detail Field
}

public class VmPurchaseDebitNotePost
{
    public VmPurchaseDebitNotePost()
    {
        vmPurchaseDebitNoteDetials = new List<VmPurchaseDebitNoteDetial>();
    }

    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public int PurchaseId { get; set; }
    public string ReasonOfReturn { get; set; }
    public DateTime ReturnDate { get; set; }
    public int VehicleTypeId { get; set; }
    public string VehicleName { get; set; }
    public string VehicleRegNo { get; set; }
    public string VehicleDriverName { get; set; }
    public string VehicleDriverContactNo { get; set; }
    public string VoucherNo { get; set; }

    public List<VmPurchaseDebitNoteDetial> vmPurchaseDebitNoteDetials { get; set; }

}