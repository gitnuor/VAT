﻿using System;

namespace vms.entity.viewModels.Payment;

public class TdsPaymentListViewModel
{
	public int TdsPaymentId { get; set; }

	public int OrganizationId { get; set; }

	public int CustomsAndVatcommissionarateId { get; set; }

	public int MushakYear { get; set; }

	public int MushakMonth { get; set; }

	public string MushakMonthName { get; set; }

	public decimal PaidAmount { get; set; }

	public DateTime PaymentDate { get; set; }

	public int BankId { get; set; }

	public string BankName { get; set; }

	public string BankBranchName { get; set; }

	public int BankBranchDistrictOrCityId { get; set; }

	public string DistrictOrCityName { get; set; }

	public string EconomicCode1stDisit { get; set; }

	public string EconomicCode2ndDisit { get; set; }

	public string EconomicCode3rdDisit { get; set; }

	public string EconomicCode4thDisit { get; set; }

	public string EconomicCode5thDisit { get; set; }

	public string EconomicCode6thDisit { get; set; }

	public string EconomicCode7thDisit { get; set; }

	public string EconomicCode8thDisit { get; set; }

	public string EconomicCode9thDisit { get; set; }

	public string EconomicCode10thDisit { get; set; }

	public string EconomicCode11thDisit { get; set; }

	public string EconomicCode12thDisit { get; set; }

	public string EconomicCode13thDisit { get; set; }

	public string EconomicCode { get; set; }

	public bool IsSubmitted { get; set; }

	public string IsSubmittedStatus { get; set; }

	public string TreasuryChallanNo { get; set; }

	public DateTime? SubimissionDate { get; set; }
}