using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmSalesIndex
{
    public IEnumerable<Sale> SalesList { get; set; }
    [DisplayName("Sales File")]
    [Required(ErrorMessage ="Sales Excel File is required")]
    [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
    public IFormFile UploadedFile { get; set; }
}