using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmSearchTransferRawMaterial
{
    public vmSearchTransferRawMaterial()
    {
        TransferRawMaterial = new List<ContractualProductionTransferRawMaterial>();
    }
    public IEnumerable<ContractualProductionTransferRawMaterial> TransferRawMaterial { get; set; }
    [DateShouldBeUpToToday(ErrorMessage = "From Date should not be greater than today and less than year of 2000!")]
    [DateLessThan(nameof(ToDate), AllowEquality = true, ErrorMessage = "From Date must be less than or equal to To Date.")]
    [Display(Name = "From Date")]
    public DateTime FromDate { get; set; }
    [DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
    [Display(Name = "To Date")]
    public DateTime ToDate { get; set; }
    public int PurchaseId { get; set; }
    public string InvoiceNo { get; set; }
    public int vendorId { get; set; }
    public int reason { get; set; }
    public int organizationId { get; set; }
    public string ReportUrl { get; set; }

        
}