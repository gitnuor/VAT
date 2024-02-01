using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel.HTMLMushak;

public class Sp4p3
{
    [Key]
    public string OrganizationName { get; set; }
    public string OrganizationAddress { get; set; }
    public string OrganizationBin { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public string SubmissionNo { get; set; }
    public DateTime? DateOfSubmission { get; set; }
    public DateTime FirstSupplyDate { get; set; }
    public string FinishedProductName { get; set; }
    public string FinishedProductHsCode { get; set; }
    public string FinishedProductModelNo { get; set; }
    public string FinishedProductMeasurementUnit { get; set; }
    public int? RmspcPriceSetupId { get; set; }
    public int? RawMaterialId { get; set; }
    public string RawmaterialName { get; set; }
    public decimal? RawmaterialRequiredQtyWithWastage { get; set; }
    public decimal? RawmaterialPurchasePrice { get; set; }
    public decimal? RawmaterialWastageQty { get; set; }
    public decimal? RawmaterialWastagePercentage { get; set; }
    public int? OhcpspcPriceSetupId { get; set; }
    public int? OverHeadCostId { get; set; }
    public string OverHeadCostName { get; set; }
    public decimal? OverHeadCostAmount { get; set; }
    public decimal ProfitAmount { get; set; }
}