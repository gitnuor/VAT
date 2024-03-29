﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace vms.entity.models;

public partial class ViewContractualProduction
{
    public int ContractualProductionId { get; set; }

    public long? ApiTransactionId { get; set; }

    public string ChallanNo { get; set; }

    public DateTime ClosingDate { get; set; }

    public DateTime ContractDate { get; set; }

    public string ContractNo { get; set; }

    public int? ContractTypeId { get; set; }

    public int? CustomerId { get; set; }

    public bool IsClosed { get; set; }

    public DateTime? IssueDate { get; set; }

    public int OrganizationId { get; set; }

    public string ReferenceKey { get; set; }

    public int? VendorId { get; set; }

    public string AccountCode { get; set; }

    public string Address { get; set; }

    public string BankAccountNo { get; set; }

    public string BankBranchAddress { get; set; }

    public int? BankBranchCountryId { get; set; }

    public string BankBranchDistrictOrCityName { get; set; }

    public string BankBranchName { get; set; }

    public int? BankId { get; set; }

    public string BankRoutingCode { get; set; }

    public string BinNo { get; set; }

    public string BusinessCategoryDescription { get; set; }

    public int? BusinessCategoryId { get; set; }

    public int? BusinessNatureId { get; set; }

    public string ContactNo { get; set; }

    public string ContactPerson { get; set; }

    public string ContactPersonDesignation { get; set; }

    public string ContactPersonEmailAddress { get; set; }

    public string ContactPersonMobile { get; set; }

    public int? CountryId { get; set; }

    public decimal? CreditLimit { get; set; }

    public int? CreditPeriodInDay { get; set; }

    public int? CustomsAndVatcommissionarateId { get; set; }

    public string DistrictOrCityName { get; set; }

    public string DivisionOrStateName { get; set; }

    public string EmailAddress { get; set; }

    public long? ExcelDataUploadId { get; set; }

    public bool? IsForeignVendor { get; set; }

    public bool? IsRegistered { get; set; }

    public bool? IsRegisteredAsTurnOverOrg { get; set; }

    public bool? IsRegisteredUnderActNinetyFour { get; set; }

    public bool? IsRequireBranch { get; set; }

    public bool? IsTds { get; set; }

    public bool? IsVds { get; set; }

    public string VendorName { get; set; }

    public string NationalIdNo { get; set; }

    public string PostCode { get; set; }

    public string RegistrationNumberUnderActNinetyFour { get; set; }

    public string ServiceVatCode { get; set; }

    public decimal? Tdsrate { get; set; }

    public string Tinno { get; set; }

    public decimal? Vdsrate { get; set; }

    public string VendorCode { get; set; }

    public int? VendorTypeId { get; set; }

    public string ContractTypeName { get; set; }

    public string OrganizationName { get; set; }

    public string OrganizationAddress { get; set; }

    public string OrganizationBin { get; set; }

    public int ContractualProductionProductDetailsId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal Quantity { get; set; }

    public int MeasurementUnitId { get; set; }

    public string UnitName { get; set; }
}