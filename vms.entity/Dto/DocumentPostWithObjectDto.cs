using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using vms.entity.CustomDataAnnotations;

namespace vms.entity.Dto;

public class DocumentPostWithObjectDto
{
    [DisplayName("Document")]
    [Required]
    public string DocumentId { get; set; }
    [DisplayName("Document Type")]
    [Required]
    public string DocumentTypeId { get; set; }
    [DisplayName("File")]
    [Required]
    [MaxFileSize(1024 * 1024)]
    [AllowedExtensions(new[] { ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".csv" })]
    public IFormFile UploadedFile { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string DocumentRemarks { get; set; }
}