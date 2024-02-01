using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels;

public class VmSelfProduction
{
    public VmSelfProduction()
    {
        SelfProductionBillMaterialList = new List<VmSelfProductionBillMaterial>();
        SelfProductionDocumentList = new List<VmSelfProductionDocument>();
        MeasurementUnitSelectList = new List<SelectListItem>();
        ProductSelectList = new List<SpGetProductForSelfProductionReceive>();
        DocumentTypeSelectList = new List<CustomSelectListItem>();
        BranchSelectList = new List<CustomSelectListItem>();
    }
    public int CreatedBy { get; set; }
    public int OrganizationId { get; set; }

    [DisplayName("Branch")]
    [Required]
    public int OrgBranchId { get; set; }


    [DisplayName("Batch Number")]
    [MaxLength(20)]
    public string BatchNo { get; set; }
    [Required(ErrorMessage = "Product Name is Required")]
    [DisplayName("Product Name")]
    public int ProductId { get; set; }
    [DisplayName("RCV Quantity")]
    [Required(ErrorMessage = "RCV Quantity is Reuired")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal ReceiveQuantity { get; set; }
    [DisplayName("M. Unit")]
    //[Required(ErrorMessage = "M. Unit is Required")]
    public string MeasurementName { get; set; }
	[DisplayName("M. Unit")]
	[Required(ErrorMessage = "M. Unit is Required")]
	public int MeasurementUnitId { get; set; }
    [DisplayName("RCV. Time")]
    [Required(ErrorMessage = "RCV. Time is Required")]
    public DateTime ReceiveTime { get; set; }

    #region Self Production Bill Material
    [DisplayName("Raw Metarial")]
    public int RawMaterialId { get; set; }
    [DisplayName("Used Qty")]
    public decimal UsedQuantity { get; set; }
    [DisplayName("M. Unit")]
    public int BmMeasurementUnitId { get; set; }
    [DisplayName("Max Qty")]
    public double MaxQty { get; set; }
    #endregion

    #region Self Production Document
    [DisplayName("Document Type")]
    [Required(ErrorMessage ="DocumentType is Required")]
    public int DocumentTypeId { get; set; }
    [NotMapped]
    [Required(ErrorMessage ="File is Required")]
    public IFormFile UploadedFile { get; set; }
    [DisplayName("File")]
    public string FilePath { get; set; }

    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string DocumentRemarks { get; set; }
    #endregion

    public List<VmSelfProductionBillMaterial> SelfProductionBillMaterialList { get; set; }
    public List<VmSelfProductionDocument> SelfProductionDocumentList { get; set; }

    public IEnumerable<SelectListItem> MeasurementUnitSelectList { get; set; }
    public List<SpGetProductForSelfProductionReceive> ProductSelectList { get; set; }
    public IEnumerable<CustomSelectListItem> DocumentTypeSelectList { get; set; }
    public IEnumerable<CustomSelectListItem> BranchSelectList { get; set; }
}


public class VmSelfProductionBillMaterial
{
    [DisplayName("Raw Metarial")]
    public int RawMaterialId { get; set; }
    [DisplayName("Used Qty")]
    public decimal UsedQuantity { get; set; }
    [DisplayName("M. Unit")]
    public int MeasurementUnitId { get; set; }
    [DisplayName("Max Qty")]
    public double MaxQty { get; set; }
}


public class VmSelfProductionDocument
{
    [DisplayName("Document Type")]
    public int DocumentTypeId { get; set; }
    [NotMapped]
    public IFormFile UploadedFile { get; set; }
    public string FileUrl => UploadedFile.Name;
    public string MimeType => UploadedFile.Name;
    [DisplayName("File")]
    public string FilePath { get; set; }

    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string DocumentRemarks { get; set; }
}