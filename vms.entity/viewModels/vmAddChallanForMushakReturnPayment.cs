using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;

namespace vms.entity.viewModels;

public class vmAddChallanForMushakReturnPayment
{
    public int MushakReturnPaymentId { get; set; }
    [DisplayName("Treasury Challan No.")]
    [Required(ErrorMessage = "Treasury Challan No. is required")]
    [StringLength(40, ErrorMessage = "Treasury Challan No. can not have more than 40 characters")]
    public string TreasuryChallanNo { get; set; }
    [DateShouldBeUpToToday(ErrorMessage = "Submission Date should not be greater than today and less than year of 2000!")]
    [DisplayName("Submission Date")]
    [Required(ErrorMessage ="Submission date is required")]
    public DateTime SubmissionDate { get; set; }
}