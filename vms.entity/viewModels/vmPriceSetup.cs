using System;
using System.Collections.Generic;

namespace vms.entity.viewModels;

public class vmPriceSetup
{
    public int PriceSetupId { get; set; }
    public int OrganizationId { get; set; }
    public int? ProductId { get; set; }
    public int? MeasurementUnitId { get; set; }
    public decimal? BaseTp { get; set; }
    public decimal? Mrp { get; set; }
    public decimal PurchaseUnitPrice { get; set; }
    public decimal SalesUnitPrice { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsActive { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public IEnumerable<CustomSelectListItem> ProductVattype;
    public List<RawMaterial> RawMaterialLists { get; set; }
    public List<OverHeadDetails> OverHeadCostsList { get; set; }

    public class OverHeadDetails
    {
        public int OverHeadCostId { get; set; }
        public decimal cost { get; set; }
           
    }
    public class RawMaterial
    {
        public int? productId { get; set; }
        public decimal? RequireQty { get; set; }
        public int? MeasurementUnitId { get; set; }
        public int? WastagePercentage { get; set; }
        public int? OverHeadCostId { get; set; }
        public decimal cost { get; set; }
        public bool IsRawMaterial { get; set; }
    }
}