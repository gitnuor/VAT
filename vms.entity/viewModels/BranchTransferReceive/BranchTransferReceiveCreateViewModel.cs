using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels.BranchTransferReceive;

public class BranchTransferReceiveCreateViewModel
{
    public BranchTransferReceiveCreateViewModel()
    {
        ProductForTransferList = new List<SpGetProductForSale>();
        MeasurementUnitSelectList = new List<SelectListItem>();
        DocumentTypeSelectList = new List<SelectListItem>();
        VehicleTypesList = new List<VehicleType>();
        OrgBranchList = new List<OrgBranch>();
    }
    
    public IEnumerable<OrgBranch> OrgBranchList { get; set; }
    public IEnumerable<SpGetProductForSale> ProductForTransferList { get; set; }
    public IEnumerable<SelectListItem> MeasurementUnitSelectList { get; set; }
    public IEnumerable<SelectListItem> DocumentTypeSelectList { get; set; }
    public IEnumerable<VehicleType> VehicleTypesList { get; set; }

    

    #region Vehicle Information
    [DisplayName("Driver Name")]
    [MaxLength(50)]
    public string DriverName { get; set; }
    [DisplayName("Driver Mobile")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string DriverMobile { get; set; }
    [DisplayName("Is Require Registration")]
    public Boolean IsRequireRegistration { get; set; }
    [DisplayName("Vehicle Type")]
    public int VehicleTypeId { get; set; }
    [DisplayName("Vehicle Registration No")]
    [MaxLength(50)]
    public string VehicleRegistrationNo { get; set; }
    #endregion
    
    public string OrganizationName { get; set; }
    public string OrganizationBin { get; set; }
    public string OrganizationAddress { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public int BranchTransferReceiveId { get; set; }
    [DisplayName("Sender Branch")]
	[Required(ErrorMessage = "{0} is Required")]
    public int SenderBranchId { get; set; }
    [Required(ErrorMessage = "{0} is Required")]
    [DisplayName("Receiver Branch")]
    public int ReceiverBranchId { get; set; }
    [MaxLength(50, ErrorMessage = "Max Length 50 allowed")]
    [DisplayName("Invoice No.")]
    public string InvoiceNo { get; set; }

    [DisplayName("Invoice Date")]
    public DateTime? InvoiceDate { get; set; }
    [Required(ErrorMessage = "Del. Date is Required")]
    [DisplayName("Del. Date")]
    public DateTime? DeliveryDate { get; set; }
    // [Required(ErrorMessage = "Rec. Name is Required")]
    [DisplayName("Rec. Name")]
    [MaxLength(50)]
    public string ReceiverName { get; set; }
    [DisplayName("Rec. Contact No.")]
    [RegularExpression(@"^(01[3-9][0-9]{8})$", ErrorMessage = "Mobile No should be at form 01xxxxxxxxx!!!")]
    public string ReceiverContactNo { get; set; }
    [Required(ErrorMessage = "Del. Address is Required")]
    [DisplayName("Del. Address")]
    [MaxLength(100)]
    public string ShippingAddress { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(500)]
    public string BranchTransferReceiveRemarks { get; set; }


    #region Transfer Detail

    public int BranchTransferReceiveDetailId { get; set; }
    [Required(ErrorMessage = "Product is Required")]
    [DisplayName("Product")]
    public int ProductId { get; set; }

    [DisplayName("Description")]
    [MaxLength(500)]
    public string ProductDescription { get; set; }

    [DisplayName("HS Code")]
    [MaxLength(50)]
    public string Hscode { get; set; }

    [DisplayName("Product Code")]
    [MaxLength(50)]
    public string ProductCode { get; set; }

    [DisplayName("SKU")]
    [MaxLength(50)]
    public string SKUNo { get; set; }

    [DisplayName("SKU ID")]
    [MaxLength(50)]
    public string SKUId { get; set; }

    [DisplayName("GOODS ID")]
    [MaxLength(50)]
    public string GoodsId { get; set; }
    public long? ProductTransactionBookId { get; set; }
    [DisplayName("Quantity")]
    [Required(ErrorMessage = "Quantity is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
    public decimal Quantity { get; set; }
    [Required(ErrorMessage ="Current Stock is Required")]
    [DisplayName("Cur. Stock")]
    public decimal CurrentStock { get; set; }


    [DisplayName("Unit Price")]
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
    public decimal UnitPrice { get; set; }

    [DisplayName("Total Price")]
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
    public decimal TotalPrice { get; set; }

    [DisplayName("M. Unit")]
    public int MeasurementUnitId { get; set; }
    [DisplayName("M. Unit")]
    public string MeasurementUnitName { get; set; }
    [DisplayName("Remarks")]
    public string ProductRemarks { get; set; }

    //Extra Field End Here
    #endregion

    #region Branch Transfer Document

    [DisplayName("Type")]
    [Required(ErrorMessage = "DocumentType is Required")]
    public int DocumentType { get; set; }
    [NotMapped,Required(ErrorMessage ="File is Required")]
    public IFormFile FileUpload { get; set; }

    public string FilePath { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string DocumentRemarks { get; set; }
    #endregion
}