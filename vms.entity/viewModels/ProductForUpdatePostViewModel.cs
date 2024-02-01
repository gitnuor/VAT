using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels;

public class ProductForUpdatePostViewModel
{

    public int ProductId { get; set; }

    [Display(Name = "Product/Service Name")]
    [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
    [Required]
    public string Name { get; set; }

    [DisplayName("Brand")]
    public int? BrandId { get; set; }

    [DisplayName("Product Number")]
    [StringLength(50, ErrorMessage = "Product Number cannot be longer than 50 characters.")]
    public string ProductNumber { get; set; }

    [Display(Name = "Model No")]
    [StringLength(50, ErrorMessage = "Model No cannot be longer than 50 characters.")]
    public string ModelNo { get; set; }

    [Display(Name = "Device Model")]
    [StringLength(50, ErrorMessage = "Device Model cannot be longer than 50 characters.")]
    public string DeviceModel { get; set; }

    [Display(Name = "Item Code")]
    [StringLength(50, ErrorMessage = "Item Code cannot be longer than 50 characters.")]
    public string Code { get; set; }

    [Display(Name = "Part No")]
    [StringLength(50, ErrorMessage = "Part No cannot be longer than 50 characters.")]
    public string PartNo { get; set; }

    [Display(Name = "Part Code")]
    [StringLength(50, ErrorMessage = "Part Code cannot be longer than 50 characters.")]
    public string PartCode { get; set; }

    [Display(Name = "Variant")]
    [StringLength(50, ErrorMessage = "Variant cannot be longer than 50 characters.")]
    public string Variant { get; set; }

    [Display(Name = "Color")]
    [StringLength(50, ErrorMessage = "Color cannot be longer than 50 characters.")]
    public string Color { get; set; }

    [Display(Name = "Size")]
    [StringLength(50, ErrorMessage = "Size cannot be longer than 50 characters.")]
    public string Size { get; set; }

    [Display(Name = "Weight")]
    [StringLength(50, ErrorMessage = "Weight cannot be longer than 50 characters.")]
    public string Weight { get; set; }

    [Display(Name = "Weight In Kg")]
    public decimal? WeightInKg { get; set; }

    [Display(Name = "Specification")]
    [StringLength(100, ErrorMessage = "Specification cannot be longer than 100 characters.")]
    public string Specification { get; set; }

    [Display(Name = "HS/Service code")]
    [StringLength(50, ErrorMessage = "HSCode cannot be longer than 50 characters.")]
    [Required]
    public string Hscode { get; set; }

    [Display(Name = "Product/Service Category")]

    public int? ProductCategoryId { get; set; }

    [DisplayName("Product/Service Group")]
    [Required(ErrorMessage = "Product Group is Required")]
    public int ProductGroupId { get; set; }

    [DisplayName("Organization")]
    public int OrganizationId { get; set; }

    [DisplayName("Total Quantity")]
    public decimal TotalQuantity { get; set; }

    [DisplayName("Measurement Unit")]
    [Required(ErrorMessage = "Measurement Unit is Required")]
    public int MeasurementUnitId { get; set; }

    [Display(Name = "Description")]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
    public string Description { get; set; }

    public string ReferenceKey { get; set; }

    public bool IsNonRebateable { get; set; }

    [DisplayName("Status")]
    public bool IsActive { get; set; }

    [DisplayName("Saleable")]
    public bool IsSellable { get; set; }

    [DisplayName("Is Raw Material?")]
    public bool IsRawMaterial { get; set; }
    [DisplayName("Product Type")]
    [Required(ErrorMessage = "Product Type is Required")]
    public int ProductTypeId { get; set; }
    public IEnumerable<CustomSelectListItem> ProductTypeSelectListItems { get; set; } = new List<CustomSelectListItem>();
    public IEnumerable<CustomSelectListItem> BrandsSelectListItems { get; set; }
    public IEnumerable<ProductVattype> ProductVatTypeList { get; set; }
    public IEnumerable<CustomSelectListItem> ProductCategories { get; set; }

    public IEnumerable<CustomSelectListItem> MeasurementUnits;
    public IEnumerable<CustomSelectListItem> ProductGroups;
}
