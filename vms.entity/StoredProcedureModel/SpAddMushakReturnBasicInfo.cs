using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.StoredProcedureModel;

public class SpAddMushakReturnBasicInfo
{
    public int? OrganizationId { get; set; }
    [Required]
    public int? Year { get; set; }
    [Required]
    public int? Month { get; set; }
    public DateTime? GenerateDate { get; set; }
    public decimal? InterestForDueVat { get; set; }
    public decimal? InterestForDueSd { get; set; }
    public decimal? FinancialPenalty { get; set; }
    public decimal? ExciseDuty { get; set; }
    public decimal? DevelopmentSurcharge { get; set; }
    public decimal? ItDevelopmentSurcharge { get; set; }
    public decimal? HealthDevelopmentSurcharge { get; set; }
    public decimal? EnvironmentProtectSurcharge { get; set; }
    public decimal? MiscIncrementalAdjustmentAmount { get; set; }
    public decimal? MiscIncrementalAdjustmentDesc { get; set; }
    public decimal? MiscDecrementalAdjustmentAmount { get; set; }
    public decimal? MiscDecrementalAdjustmentDesc { get; set; }
    public bool? IsWantToGetBackClosingAmount { get; set; }
}