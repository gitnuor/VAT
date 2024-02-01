using System;

namespace vms.entity.StoredProcedureModel.ProductReport;

public class SpGetProductCurrentStock
{
	public int OrganizationId { get; set; }
	public string OrganizationName { get; set; }
	public string OrganizationAddress { get; set; }
	public string OrganizationBin { get; set; }
	public string BranchName { get; set; }
	public string BranchAddress { get; set; }
	public int? ProductId { get; set; }
	public string ProductName { get; set; }
	public int? BrandId { get; set; }
	public string BrandName { get; set; }
	public string HsCode { get; set; }
	public int? ProductCategoryId { get; set; }
	public string ProductCategoryName { get; set; }
	public int? ProductGroupId { get; set; }
	public string ProductGroupName { get; set; }
	public int? ProductTypeId { get; set; }
	public string FilteredProductTypeName { get; set; }
	public string ProductTypeName { get; set; }
	public string ProductNumber { get; set; }
	public string ModelNo { get; set; }
	public string DeviceModel { get; set; }
	public string Code { get; set; }
	public string PartNo { get; set; }
	public string PartCode { get; set; }
	public string Variant { get; set; }
	public string Color { get; set; }
	public string Size { get; set; }
	public string Weight { get; set; }
	public decimal? WeightInKg { get; set; }
	public string Specification { get; set; }
	public decimal? TotalQuantity { get; set; }
	public decimal? TotalWeightInKg { get; set; }
	public string MeasurementUnitName { get; set; }
	public int? MeasurementUnitId { get; set; }
	public DateTime? GeneratedTime { get; set; }
		
}