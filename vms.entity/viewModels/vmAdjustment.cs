using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.entity.viewModels;

public class vmAdjustment : VmMushakReturnDisplay
{
    public int AdjustmentId { get; set; }
    [Display(Name = "Adjustment Type")]
    public int AdjustmentTypeId { get; set; }
    [Range(.01, 99999999999, ErrorMessage = "Amount should be between 0.01 to 99999999999")]
    public decimal Amount { get; set; }
    public bool IsActive { get; set; }
    [Required]
    [MinLength(2)]
    [MaxLength(500)]
    public string Description { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public IEnumerable<CustomSelectListItem> AdjustmentTypes;

}