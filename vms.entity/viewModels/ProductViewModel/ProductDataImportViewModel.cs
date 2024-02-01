using System;

namespace vms.entity.viewModels.ProductViewModel;

public class ProductDataImportViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string ModelNo { get; set; }
    public string Code { get; set; }
    public string Hscode { get; set; }
    public int? ProductCategoryId { get; set; }
    public string ProductCategoryName { get; set; }
    public int ProductGroupId { get; set; }
    public string ProductGroupsName { get; set; }
    public int OrganizationId { get; set; }
    public decimal TotalQuantity { get; set; }
    public string MeasurementUnitName { get; set; }
    public int MeasurementUnitId { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsSellable { get; set; }
    public bool IsRawMaterial { get; set; }
    public bool? IsNonRebateable { get; set; }
    public string ReferenceKey { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedTime { get; set; }
    public long? ApiTransactionId { get; set; }
}