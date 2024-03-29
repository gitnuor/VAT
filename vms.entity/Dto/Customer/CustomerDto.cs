﻿using System;

namespace vms.entity.Dto.Customer;

public class CustomerDto
{
    public int CustomerId { get; set; }
    public int OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public string OrganizationAddress { get; set; }

    public string OrganizationBin { get; set; }

    public string CustomerName { get; set; }

    public string AccountCode { get; set; }

    public string BinNo { get; set; }

    public string NidNo { get; set; }

    public string TinNo { get; set; }

    public bool? IsVds { get; set; }

    public string IsVdsStatus { get; set; }

    public decimal? VdsRate { get; set; }

    public bool? IsTds { get; set; }

    public string IsTdsStatus { get; set; }

    public decimal? TdsRate { get; set; }

    public decimal? CreditLimit { get; set; }

    public int? CreditPeriodInDay { get; set; }

    public bool? IsRegisteredUnderActNinetyFour { get; set; }

    public string RegistrationNumberUnderActNinetyFour { get; set; }

    public int? CustomsAndVatCommissionarateId { get; set; }

    public string CustomsAndVatCommissionarateName { get; set; }

    public string ServiceVatCode { get; set; }

    public int? CountryId { get; set; }

    public string ConuntryName { get; set; }

    public string DistrictOrCityName { get; set; }

    public string DivisionOrStateName { get; set; }

    public string CustomerAddress { get; set; }

    public string PostCode { get; set; }

    public string PhoneNo { get; set; }

    public string EmailAddress { get; set; }

    public int? BusinessNatureId { get; set; }

    public string BusinessNatureName { get; set; }

    public int? BusinessCategoryId { get; set; }

    public string BusinessCategoryName { get; set; }

    public string BusinessCategoryDescription { get; set; }

    public bool IsForeignCustomer { get; set; }

    public string IsForeignCustomerStatus { get; set; }

    public bool IsFullExportOriented { get; set; }

    public string IsFullExportOrientedStatus { get; set; }

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

    public bool? IsRequireBranch { get; set; }

    public string IsRequireBranchStatus { get; set; }

    public string ReferenceKey { get; set; }

    public bool IsActive { get; set; }

    public string IsActiveStatus { get; set; }

    public DateTime? CreatedTime { get; set; }
}