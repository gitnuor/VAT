using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.CustomerViewModel;

public class CustomerSubscriptionDetailCreateViewModel
{
	public int CustomerSubscriptionDetailId { get; set; }

	public int CustomerSubscriptionId { get; set; }

	public int ProductId { get; set; }

	public string ProductName { get; set; }

	public int MeasurementUnitId { get; set; }

	public string MeasurementUnitName { get; set; }

	public decimal ConversionRatio { get; set; }

	public decimal Quantity { get; set; }

	public decimal UnitPrice { get; set; }

	public decimal SupplementaryDutyPercent { get; set; }

	public int ProductVatTypeId { get; set; }

	public string ProductVatTypeName { get; set; }

	public decimal ProductVatPercent { get; set; }
}