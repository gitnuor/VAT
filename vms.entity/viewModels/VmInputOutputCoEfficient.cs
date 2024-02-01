using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.StoredProcedureModel;

namespace vms.entity.viewModels;

public class VmInputOutputCoEfficient
{
    public VmInputOutputCoEfficient()
    {
        MeasurementUnitSelectList = new List<CustomSelectListItem>();
        RawMaterialForInputOutputCoEfficientList = new List<SpGetRawMaterialForInputOutputCoEfficient>();
        OverheadCostSelectList = new List<SelectListItem>();

    }

    public IEnumerable<CustomSelectListItem> MeasurementUnitSelectList { get; set; }
    public IEnumerable<SpGetRawMaterialForInputOutputCoEfficient> RawMaterialForInputOutputCoEfficientList { get; set; }
    public IEnumerable<SelectListItem> OverheadCostSelectList { get; set; }

    #region Product Cost
    [DisplayName("Product")]
    [Required(ErrorMessage = "Product is Required")]
    public int ProductId { get; set; }
    [DisplayName("Product")]
    public int HiddenProductId { get; set; }
    [DisplayName("Sales Unit Price")]
    public decimal SalesUnitPrice { get; set; }
    [DisplayName("Qty")]
    [Range(0.00000001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
    [Required(ErrorMessage = "Qty is Required")]
    public decimal RequiredQty { get; set; }
    [DisplayName("Cost")]
    [Range(0.00000001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
    [Required(ErrorMessage = "Cost is Required")]
    public decimal ProductCost { get; set; }
    [DisplayName("Unit Price")]
    public decimal UnitPrice { get; set; }
    [DisplayName("M. Unit")]
    public int MeasurementUnitId { get; set; }
    [DisplayName("M. Unit")]
    public string MeasurementUnitName { get; set; }
    [DisplayName("Wastage")]
    [Range(0, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal WastagePercentage { get; set; }

    #endregion

    #region Details View
    [DisplayName("Product Name")]
    public string ProductName { get; set; }
    [DisplayName("Model")]
    public string ModelNo { get; set; }
    [DisplayName("Size")]
    public string Size { get; set; }
    [DisplayName("Product Group Name")]
    public string ProductGroupName { get; set; }
    [DisplayName("Product Category Name")]
    public string ProductCategoryName { get; set; }
    [DisplayName("Hs Code")]
    public string HSCode { get; set; }
    [DisplayName("Weight")]
    public string Weight { get; set; }
    [DisplayName("Brand")]
    public string BrandName { get; set; }
    [DisplayName("Specification")]
    public string Specification { get; set; }
    [DisplayName("Product Type")]
    public string ProductTypeName { get; set; }
    [DisplayName("Variant")]
    public string Variant { get; set; }
    [DisplayName("Color")]
    public string Color { get; set; }
    [DisplayName("Code")]
    public string Code { get; set; }
    #endregion

    #region OverheadCost
    [DisplayName("Overhead Cost")]
    [Required(ErrorMessage = "Item is Required")]
    public int OverheadCostId { get; set; }
    [DisplayName("Overhead Cost")]
    [Required(ErrorMessage = "Cost is Required")]
    [Range(0.00000001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
    public decimal OverheadCostAmount { get; set; }
    public string Name { get; set; }

    #endregion

    #region Profit Calculation
    [Range(int.MinValue, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [Required(ErrorMessage = "Profit Amount is Required")]
    public decimal ProfitAmount { get; set; }
    public decimal ProfitPercent { get; set; }
    #endregion

}


public class VmInputOutputCoEfficientPost
{
    public VmInputOutputCoEfficientPost()
    {
        ProductCostList = new List<InputOutputCoEfficientProductCost>();
        OverheadCostList = new List<InputOutputCoEfficientOverheadCost>();
    }

    [DisplayName("Product")]
    public int HiddenProductId { get; set; }
    public int MeasurementUnitId { get; set; }

    [DisplayName("Sales Unit Price")]
    public decimal SalesUnitPrice { get; set; }

    #region Profit Calculation
    public decimal ProfitAmount { get; set; }
    public decimal ProfitPercent { get; set; }
    #endregion
    public List<InputOutputCoEfficientProductCost> ProductCostList { get; set; }
    public List<InputOutputCoEfficientOverheadCost> OverheadCostList { get; set; }
}

public class InputOutputCoEfficientProductCost
{
    [DisplayName("Product")]
    public int ProductId { get; set; }
    [DisplayName("Sales Unit Price")]
    public decimal SalesUnitPrice { get; set; }
    [DisplayName("Qty")]
    public decimal RequiredQty { get; set; }
    [DisplayName("Cost")]
    public decimal ProductCost { get; set; }
    [DisplayName("Unit Price")]
    public decimal UnitPrice { get; set; }
    [DisplayName("M. Unit")]
    public int MeasurementUnitId { get; set; }
    [DisplayName("M. Unit")]
    public string MeasurementUnitName { get; set; }
    [DisplayName("Wastage")]
    public decimal WastagePercentage { get; set; }
}

public class InputOutputCoEfficientOverheadCost
{
    [DisplayName("Overhead Cost")]
    public int OverheadCostId { get; set; }
    [DisplayName("Overhead Cost")]
    public decimal OverheadCostAmount { get; set; }
    public string Name { get; set; }
}