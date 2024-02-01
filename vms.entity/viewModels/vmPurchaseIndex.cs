using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmPurchaseIndex
{
    public IEnumerable<Purchase> PurchaseList { get; set; }
    [Required(ErrorMessage = "Purchase Excel File is required")]
    [DisplayName("Purchase File")]
    [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
    public IFormFile UploadedFile { get; set; }
        
}