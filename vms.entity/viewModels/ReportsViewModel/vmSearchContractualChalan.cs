using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmSearchContractualChalan
{
    public vmSearchContractualChalan()
    {
        TransferRawMaterial = new List<ContractualProductionTransferRawMaterial>();
        ContractChallanList = new List<ContractualProduction>();
    }
    public IEnumerable<ContractualProductionTransferRawMaterial> TransferRawMaterial { get; set; }
    public IEnumerable<ContractualProduction> ContractChallanList { get; set; }
    [DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
    [DateLessThan(nameof(ToDate), AllowEquality = true, ErrorMessage = "From Date must be less than or equal to To Date.")]
    [Display(Name = "From Date")]
    public DateTime FromDate { get; set; }
    [DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
    [Display(Name = "To Date")]
    public DateTime ToDate { get; set; }
              
}