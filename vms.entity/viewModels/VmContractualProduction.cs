using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels;

public class VmContractualProduction
{

    public VmContractualProduction()
    {
        ContractualProductionBillMaterialList = new List<VmContractualProductionBillMaterial>();
        ContractualProductionDocumentList = new List<VmContractualProductionDocument>();
        ContractualProductionContactSelectList = new List<SelectListItem>();
        ProductSelectList = new List<SpGetProductForContractualProductionReceive>();
        DocumentTypeSelectList = new List<CustomSelectListItem>();
    }


    [DisplayName("Batch Number")]
    [MaxLength(20)]
    public string BatchNo { get; set; }
    [Required(ErrorMessage = "Product Name is Required")]
    [DisplayName("Product Name")]
    public int ProductId { get; set; }
    [DisplayName("Receive Quantity")]
    [Required(ErrorMessage = "ReceiveQuantity is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal ReceiveQuantity { get; set; }
    [DisplayName("M. Unit")]
    [Required(ErrorMessage = "M. Unit is Required")]
    public string MeasurementName { get; set; }
    public int MeasurementUnitId { get; set; }
    [DisplayName("Receive Time")]
    [Required(ErrorMessage = "Receive Time is Required")]
    public DateTime ReceiveTime { get; set; }

    #region Contractual Production Bill Material
    [DisplayName("Raw Metarial")]
    public int RawMaterialId { get; set; }
    [DisplayName("Used Qty.")]
    public decimal UsedQuantity { get; set; }
    [DisplayName("M. Unit")]
    public int BmMeasurementUnitId { get; set; }
    [DisplayName("Max Qty.")]
    public double MaxQty { get; set; }

    [DisplayName("Contract Number")]
    [Required(ErrorMessage = "Contract Number is Required")]
    public int ContractualProductionId { get; set; }
    public string ContractNo { get; set; }
    [DisplayName("Challan Number")]
    [MaxLength(20)]
    public string ChallanNo { get; set; }
    #endregion

    #region Contractual Production Document
    [DisplayName("Document Type")]
    [Required(ErrorMessage = "DocumentType is Required")]
    public int DocumentTypeId { get; set; }
    [NotMapped]
    [Required(ErrorMessage = "File is Required")]
    public IFormFile UploadedFile { get; set; }
    [DisplayName("File")]
    public string FilePath { get; set; }

    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string DocumentRemarks { get; set; }
    #endregion

    public List<VmContractualProductionBillMaterial> ContractualProductionBillMaterialList { get; set; }
    public List<VmContractualProductionDocument> ContractualProductionDocumentList { get; set; }

    public IEnumerable<SelectListItem> ContractualProductionContactSelectList { get; set; }
    public List<SpGetProductForContractualProductionReceive> ProductSelectList { get; set; }
    public IEnumerable<CustomSelectListItem> DocumentTypeSelectList { get; set; }
}


public class VmContractualProductionBillMaterial
{
    [DisplayName("Raw Metarial")]
    public int RawMaterialId { get; set; }
    [DisplayName("Used Qty.")]
    public decimal UsedQuantity { get; set; }
    [DisplayName("M. Unit")]
    public int MeasurementUnitId { get; set; }
    [DisplayName("Max Qty.")]
    public double MaxQty { get; set; }
}


public class VmContractualProductionDocument
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