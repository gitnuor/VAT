using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace vms.entity.viewModels;

public class VmSingleFileUpload
{
    [Required]
    public IFormFile UploadedFile { get; set; }
}