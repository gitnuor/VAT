using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.models;
using vms.entity.Utility;
using vms.entity.viewModels.ReportsViewModel;

namespace vms.entity.viewModels.Payment;

public class VmTdsPayment
{
    public VmTdsPayment()
    {
        purchaseList = new List<Purchase>();
        CountryList = new List<CustomSelectListItem>();
        DistrictOrCItyList = new List<CustomSelectListItem>();
        CustomsAndVATCommissionarate = new List<CustomsAndVatcommissionarate>();
    }

    public IEnumerable<CustomSelectListItem> CountryList;
    public IEnumerable<CustomSelectListItem> DistrictOrCItyList;
    public IEnumerable<Purchase> purchaseList { get; set; }

    [Display(Name = "Bank Branch Country")]
    [Required]
    public int BankBranchCountryId { get; set; }

    [Display(Name = "Bank Branch District/City")]
    [Required]
    public int BankBranchDistrictOrCityId { get; set; }

    [Range(minimum: 2000, maximum: 2050, ErrorMessage = "Value Must be Between 2021 and 2050")]
    [Required(ErrorMessage = "Year is Required")]
    public int Year { get; set; }
    [Required(ErrorMessage = "Month is Required")]
    public int Month { get; set; }
    [Required(ErrorMessage = "Language is Required")]
    public int Language { get; set; }
    public VmMushakReturn VmMushakReturn { get; set; }
    public List<SelectListItem> YearList { get; set; }
    public List<SelectListItem> MonthList { get; set; }
    public List<SelectListItem> LanguageList { get; set; }

    public int MushakReturnPaymentId { get; set; }
    public int OrganizationId { get; set; }
    [Display(Name = "Customs And VAT Commissionarate")]
    [Required(ErrorMessage = "Customs And VAT Commissionarate is required")]
    public int CustomsAndVatcommissionarateId { get; set; }
    [Display(Name = "Year")]
    public int MushakYear { get; set; }
    [Display(Name = "Month")]
    public int MushakMonth { get; set; }
    [Display(Name = "Payment Type")]
    public int MushakReturnPaymentTypeId { get; set; }
    [Display(Name = "Paid Amount")]
    [Required(ErrorMessage = "Paid Amount is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal PaidAmount { get; set; }
    [Display(Name = "Payment Date")]
    public DateTime PaymentDate { get; set; }
    [Display(Name = "Bank")]
    public int BankId { get; set; }
    [Display(Name = "Bank Branch")]
    [Required(ErrorMessage = "Bank Branch is required")]
    [StringLength(100, ErrorMessage = "Bank Branch can not have more than 100 characters")]
    public string BankBranchName { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public bool IsSubmitted { get; set; }
    [Display(Name = "Challan No")]
    public string TreasuryChallanNo { get; set; }
    [Display(Name = "Submission Date")]
    public DateTime? SubimissionDate { get; set; }
    public int? SubmissionEntryBy { get; set; }
    public DateTime? SubmissionEntryDate { get; set; }
    public IEnumerable<CustomsAndVatcommissionarate> CustomsAndVATCommissionarate;
    public IEnumerable<CustomSelectListItem> BankList;

    [Display(Name = "Economic Code")]
    [Required]
    [MaxLength(1)]
    [RegularExpression("[0-9]", ErrorMessage = "Only positive number and maximum 1 digit is allowed.")]
    public string EconomicCode1stDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode2ndDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode3rdDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode4thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode5thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode6thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode7thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode8thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode9thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode10thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode11thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode12thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode13thDisit { get; set; }
}

public class VmTdsPaymentForPurchase
{
	public int TdsPaymentForPurchaseId { get; set; }

	public int TdsPaymentId { get; set; }

	public int PurchaseId { get; set; }

	public decimal TdsPaidAmount { get; set; }
}


public class VmTdsPaymentPost
{
    public VmTdsPaymentPost()
    {
        paymentForTdsList = new List<VmTdsPaymentForPurchase>();
    }

    [Display(Name = "Bank Branch Country")]
    [Required]
    public int BankBranchCountryId { get; set; }

    [Display(Name = "Bank Branch District/City")]
    [Required]
    public int BankBranchDistrictOrCityId { get; set; }
    public IEnumerable<VmTdsPaymentForPurchase> paymentForTdsList { get; set; }
    [Range(minimum: 2000, maximum: 2050, ErrorMessage = "Value Must be Between 2021 and 2050")]
    [Required(ErrorMessage = "Year is Required")]
    public int Year { get; set; }
    [Required(ErrorMessage = "Month is Required")]
    public int Month { get; set; }
    [Required(ErrorMessage = "Language is Required")]
    public int Language { get; set; }
    public List<SelectListItem> YearList { get; set; }
    public List<SelectListItem> MonthList { get; set; }
    public List<SelectListItem> LanguageList { get; set; }

    public int TdsPaymentId { get; set; }
    public int OrganizationId { get; set; }
    [Display(Name = "Customs And VAT Commissionarate")]
    [Required(ErrorMessage = "Customs And VAT Commissionarate is required")]
    public int CustomsAndVatcommissionarateId { get; set; }
    [Display(Name = "Year")]
    public int MushakYear { get; set; }
    [Display(Name = "Month")]
    public int MushakMonth { get; set; }
    [Display(Name = "Paid Amount")]
    [Required(ErrorMessage = "Paid Amount is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal PaidAmount { get; set; }
    [Display(Name = "Payment Date")]
    public DateTime PaymentDate { get; set; }
    [Display(Name = "Bank")]
    public int BankId { get; set; }
    [Display(Name = "Bank Branch")]
    [Required(ErrorMessage = "Bank Branch is required")]
    [StringLength(100, ErrorMessage = "Bank Branch can not have more than 100 characters")]
    public string BankBranchName { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public bool IsSubmitted { get; set; }
    [Display(Name = "Challan No")]
    public string TreasuryChallanNo { get; set; }
    [Display(Name = "Submission Date")]
    public DateTime? SubimissionDate { get; set; }
    public int? SubmissionEntryBy { get; set; }
    public DateTime? SubmissionEntryDate { get; set; }
    public IEnumerable<CustomSelectListItem> CustomsAndVATCommissionarate;
    public IEnumerable<CustomSelectListItem> BankList;

    [Display(Name = "Economic Code")]
    [Required]
    [MaxLength(1)]
    [RegularExpression("[0-9]", ErrorMessage = "Only positive number and maximum 1 digit is allowed.")]
    public string EconomicCode1stDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode2ndDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode3rdDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode4thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode5thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode6thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode7thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode8thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode9thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode10thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode11thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode12thDisit { get; set; }
    [Display(Name = "Economic Code")]
    [Required]
    public string EconomicCode13thDisit { get; set; }
}