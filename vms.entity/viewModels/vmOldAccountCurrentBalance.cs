using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.Utility;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.entity.viewModels;

public class vmOldAccountCurrentBalance: VmMushakReturnDisplay
{
    public int OldAccountCurrentBalanceId { get; set; }
    public int OrganizationId { get; set; }

    [DisplayName("Year")]
    public int MushakYear { get; set; }
    [DisplayName("Month")]
    public int MushakMonth { get; set; }

    [DisplayName("VAT Amount")]
    [Required(ErrorMessage = "VAT Amount is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal RemainingVatbalance { get; set; }
    [DisplayName("SD. Amount")]
    [Required(ErrorMessage = "SD. Amount is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal RemainingSdbalance { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
}