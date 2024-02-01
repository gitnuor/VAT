using System;

namespace vms.entity.StoredProcedureModel;

public class SpGetProductSale
{
	public int SlNo { get; set; }
	public int OrganizationId { get; set; }
	public string OrganizationName { get; set; }
	public string OrganizationAddress { get; set; }
	public string OrganizationBin { get; set; }
	public int BranchId { get; set; }
	public string BranchName { get; set; }
	public string BranchAddress { get; set; }
	public int ProductId { get; set; }
	public string ProductName { get; set; }
	public string HsCode { get; set; }
	public int BrandId { get; set; }
	public string BrandName { get; set; }
	public int ProductCategoryId { get; set; }
	public string ProductCategoryName { get; set; }
	public int ProductGroupId { get; set; }
	public string ProductGroupName { get; set; }
	public int ProductTypeId { get; set; }
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
	public string Specification { get; set; }
	public decimal? SaleQty { get; set; }
	public decimal? SaleWeightInKg { get; set; }
	public decimal? ReturnQty { get; set; }
	public decimal? ReturnWeightInKg { get; set; }
	public decimal? OutQty { get; set; }
	public decimal? OutWeightInKg { get; set; }
	public decimal? SalePrice { get; set; }
	public decimal? ChangePrice { get; set; }
	public decimal? OutPrice { get; set; }
	public decimal? SalesVat { get; set; }
	public decimal? ChangeVat { get; set; }
	public decimal? VatAmount { get; set; }
	public decimal? SdAmount { get; set; }
	public int MeasurementUnitId { get; set; }
	public string MeasurementUnitName { get; set; }
	public DateTime GeneratedTime { get; set; }
}