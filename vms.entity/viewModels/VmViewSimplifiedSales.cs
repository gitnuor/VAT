using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using vms.entity.models;

namespace vms.entity.viewModels;

public class VmViewSimplifiedSales
{
    [Required(ErrorMessage = "File is required!")]
    public IFormFile UploadedFile { get; set; }

    public IEnumerable<ExcelDataUpload> Salses { get; set; }
}