using System.Collections.Generic;

namespace vms.entity.viewModels;

public class vmPriceSetups
{
    public IEnumerable<CustomSelectListItem> MeasurementUnits;
    public IEnumerable<CustomSelectListItem> OverHeadCost;
    public string MeasurementUnitName { get; set; }
    public string ProductGroupName { get; set; }
    public string ProductName { get; set; }
    public string ProductCategoryName { get; set; }
    public string HSCode { get; set; }
    public int ProductId { get; set; }
    public string EncryptedId { get; set; }
    public decimal SalesUnitPrice { get; set; }
    public int MeasurementUnitId { get; set; }
}