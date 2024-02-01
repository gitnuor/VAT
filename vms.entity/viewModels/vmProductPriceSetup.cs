using System;
using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmProductPriceSetup
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string ModelNo { get; set; }
    public string Code { get; set; }
    public string Hscode { get; set; }
    public int? ProductCategoryId { get; set; }
    public int ProductGroupId { get; set; }
    public int OrganizationId { get; set; }
    public decimal TotalQuantity { get; set; }
    public int MeasurementUnitId { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public bool IsSellable { get; set; }
    public bool IsRawMaterial { get; set; }
    public bool? IsNonRebateable { get; set; }
    public string OrganizationName { get; set; }
    public string MeasurementUnitName { get; set; }
    public string ProductGroupName { get; set; }
    public string ProductCategoryName { get; set; }
    public bool? IsMushakSubmitted { get; set; }
    public int? SubmissionSl { get; set; }
    public DateTime? MushakSubmissionDate { get; set; }
    public List<PriceSetup> PriceSetups { get; set; }
    public List<PriceSetupProductCost> PriceSetupProductCosts { get; set; }
}