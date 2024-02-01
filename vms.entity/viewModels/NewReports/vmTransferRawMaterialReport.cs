using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.NewReports;

public class vmTransferRawMaterialReport
{
    public vmTransferRawMaterialReport()
    {
        TransferRawMaterialList = new List<ContractualProductionTransferRawMaterial>();
    }
    public IEnumerable<ContractualProductionTransferRawMaterial> TransferRawMaterialList { get; set; }
    public HeaderModel HeaderModel { get; set; }

    [Display(Name = "From Date")]
    [DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
    [DateLessThan(nameof(ToDate), AllowEquality = true, ErrorMessage = "From Date must be less than or equal to To Date.")]
    public DateTime FromDate { get; set; }

    [Display(Name = "To Date")]
    [DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
    public DateTime ToDate { get; set; }
}