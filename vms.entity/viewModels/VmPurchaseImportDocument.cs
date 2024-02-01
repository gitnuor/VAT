using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using vms.entity.CustomDataAnnotations;

namespace vms.entity.viewModels;

public class VmPurchaseImportDocument
{
    [DisplayName("Purchase")]
    public int PurchaseId { get; set; }
    [DisplayName("Document Type")]
    [Required]
    public int DocumentTypeId { get; set; }
    [DisplayName("File")]
    [Required]
    [MaxFileSize(1024 * 1024)]
    [AllowedExtensions(new[] { ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".csv" })]
    public IFormFile UploadedFile { get; set; }
    public string FileName => UploadedFile.FileName;
    public string FileUrl => UploadedFile.FileName;
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string DocumentRemarks { get; set; }
}