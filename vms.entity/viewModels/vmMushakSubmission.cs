using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.entity.viewModels;

public class vmMushakSubmission: VmMushakReturnDisplay
{
    public int MushakSubmissionId { get; set; }

    [DisplayName("Submission Type")]
    [Required(ErrorMessage ="Submission Type is required")]
    public byte MushakSubmissionTypeId { get; set; }
    public int OrganizationId { get; set; }
    [DisplayName("Year")]
    [Required(ErrorMessage ="Year is required")]
    public int MushakForYear { get; set; }
    [DisplayName("Month")]
    [Required(ErrorMessage = "Month is required")]
    public int MushakForMonth { get; set; }
    [DisplayName("Generate Date")]
    [Required(ErrorMessage = "Generate Date is required")]
    public DateTime GenerateDate { get; set; }
    [DisplayName("Submission Date")]
    public DateTime? SubmissionDate { get; set; }

    [DisplayName("VAT Responsible Person")]
    [Required(ErrorMessage = "VAT Responsible Person is required")]
    public int VatResponsiblePersonId { get; set; }
    public bool IsWantToGetBackClosingBalance { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }

}