﻿namespace vms.entity.Dto.Customer;

public class CustomerForeignDto
{
	public int CustomerId { get; set; }

	public string CustomerName { get; set; }

	public string AccountCode { get; set; }

	public string TinNo { get; set; }

	public int? CountryId { get; set; }

	public string ConuntryName { get; set; }

	public string DistrictOrCityName { get; set; }

	public string DivisionOrStateName { get; set; }

	public string CustomerAddress { get; set; }

	public string PostCode { get; set; }

	public string PhoneNo { get; set; }

	public string EmailAddress { get; set; }

	public string ContactPerson { get; set; }

	public string ContactPersonDesignation { get; set; }

	public string ContactPersonMobile { get; set; }

	public string ContactPersonEmailAddress { get; set; }

	public int? DeliveryCountryId { get; set; }

	public string DeliveryContryName { get; set; }

	public string DeliveryDistrictOrCityName { get; set; }

	public string DeliveryDivisionOrStateName { get; set; }

	public string DeliveryAddress { get; set; }

	public string DeliveryContactPerson { get; set; }

	public string DeliveryContactPersonDesignation { get; set; }

	public string DeliveryContactPersonMobile { get; set; }

	public string DeliveryContactPersonEmailAddress { get; set; }

	public string BankAccountNo { get; set; }

	public string BankRoutingCode { get; set; }

	public int? BankId { get; set; }

	public string BankBranchName { get; set; }

	public int? BankBranchCountryId { get; set; }

	public string BankBranchCountryName { get; set; }

	public string BankBranchDistrictOrCityName { get; set; }

	public string BankBranchAddress { get; set; }

	public string ReferenceKey { get; set; }

	public bool IsActive { get; set; }

	public string IsActiveStatus { get; set; }
}