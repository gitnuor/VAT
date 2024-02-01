namespace vms.entity.StoredProcedureModel;

public  class SpGetRawMaterialForInputOutputCoEfficient
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int? BrandId { get; set; }  
    public string ProductBrandName { get; set; }
    public string ProductNumber { get; set; }
    public string ProductModel { get; set; }
    public string ProductVariant { get; set; }
    public string ProductColor { get; set; }
    public string ProductSize { get; set; }
    public string ProductWeight { get; set; }
    public string ProductSpecification { get; set; }
    public int? ProductCategoryId { get; set; }
	public string ProductCategoryName { get; set; }
    public int? ProductGroupId { get; set; }
	public string ProductGroupName { get; set; }
    public string ProductDescription { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal MaxUseQty { get; set; }
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }

    public bool IsApplicableAsRawMaterial { get; set; }

    public string ItemType { get; set; }
}