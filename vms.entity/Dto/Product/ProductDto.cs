﻿namespace vms.entity.Dto.Product;

public class ProductDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int? BrandId { get; set; }

    public string BrandName { get; set; }

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

    public string Description { get; set; }

    public string HsCodeOrServiceCode { get; set; }

    public int? ProductCategoryId { get; set; }

    public string ProductCategoryName { get; set; }

    public int ProductGroupId { get; set; }

    public string ProductGroupName { get; set; }

    public int ProductTypeId { get; set; }

    public string ProductTypeName { get; set; }

    public int MeasurementUnitId { get; set; }

    public string MeasurementUnitName { get; set; }

    public int? ProductVatid { get; set; }

    public int? ProductVattypeId { get; set; }

    public string ProductVatTypeName { get; set; }

    public bool? IsVatUpdatable { get; set; }

    public decimal? DefaultVatPercent { get; set; }

    public string ReferenceKey { get; set; }
}