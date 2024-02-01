namespace vms.entity.viewModels.BranchTransferSend;

public class SpGetBranchTransferChallanDetailModel
{
	public int SlNo { get; set; }
	public int BranchTransferSendDetailId { get; set; }
	public int BranchTransferSendId { get; set; }
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
	public int ProductTypeId { get; set; }
	public string ProductTypeName { get; set; }
	public string ProductTypeShortName { get; set; }
	public string ProductDescription { get; set; }
	public string HSCode { get; set; }
	public string ProductCode { get; set; }
	public string PartNo { get; set; }
	public string GoodsId { get; set; }
	public string SKUNo { get; set; }
	public string SKUId { get; set; }
	public decimal Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal ProductPrice { get; set; }
	public int MeasurementUnitId { get; set; }
	public string MeasurementUnitName { get; set; }
	public string ProductRemarks { get; set; }
}