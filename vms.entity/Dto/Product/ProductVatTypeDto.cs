using System;

namespace vms.entity.Dto.Product;

public class ProductVatTypeDto
{
	public int ProductVatTypeId { get; set; }

	public string Name { get; set; }

	public decimal DefaultVatPercent { get; set; }

	public bool IsApplicableForLocalPurchase { get; set; }

	public bool IsApplicableForImport { get; set; }

	public bool IsApplicableForLocalSale { get; set; }

	public bool IsApplicableForExport { get; set; }

	public bool IsRequireVds { get; set; }

	public bool IsVatUpdatable { get; set; }
}