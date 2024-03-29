﻿using System;

namespace vms.entity.Dto.SalesExport;

public class SalesExportDetailDto
{
	public int SalesDetailId { get; set; }

	public int SalesId { get; set; }

	public int ProductId { get; set; }

	public string ProductName { get; set; }

	public string ProductDescription { get; set; }

	public string HsCode { get; set; }

	public string ProductCode { get; set; }

	public string PartNo { get; set; }

	public string GoodsId { get; set; }

	public string SkuNo { get; set; }

	public string SkuId { get; set; }

	public decimal Quantity { get; set; }

	public decimal UnitPrice { get; set; }

	public int MeasurementUnitId { get; set; }

	public string MeasurementUnitName { get; set; }

	public string ReferenceKey { get; set; }

	public DateTime? CreatedTime { get; set; }
}