namespace vms.entity.viewModels.VendorViewModel;

public class VendorListDisplayViewModel
{
    public int VendorId { get; set; }
    public int? OrganizationId { get; set; }
    public int? VendorOrganizationId { get; set; }
    public string Name { get; set; }
    public string BinNo { get; set; }
    public string NationalIdNo { get; set; }
    public string Tinno { get; set; }
    public bool? IsVds { get; set; }
    public decimal? Vdsrate { get; set; }
    public bool? IsTds { get; set; }
    public decimal? Tdsrate { get; set; }
    public bool? IsRegisteredUnderActNinetyFour { get; set; }
    public string RegistrationNumberUnderActNinetyFour { get; set; }
    public int? CustomsAndVatcommissionarateId { get; set; }
    public string ServiceVatCode { get; set; }
    public int? CountryId { get; set; }
    public int? DistrictOrCityId { get; set; }
    public int? DivisionOrStateId { get; set; }
    public string Address { get; set; }
    public string PostCode { get; set; }
    public string ContactNo { get; set; }
    public string EmailAddress { get; set; }
    public bool IsActive { get; set; }
    public string ContactPerson { get; set; }
    public string ContactPersonDesignation { get; set; }
    public string ContactPersonMobile { get; set; }
    public string ContactPersonEmailAddress { get; set; }
    public string BankAccountNo { get; set; }
    public string BankRoutingCode { get; set; }
    public int? BankId { get; set; }
    public string BankBranchName { get; set; }
    public int? BankBranchCountryId { get; set; }
    public int? BankBranchDistrictOrCityId { get; set; }
    public string BankBranchAddress { get; set; }
    public int? BusinessNatureId { get; set; }
    public int? BusinessCategoryId { get; set; }
    public string BusinessCategoryDescription { get; set; }
    public bool IsRegisteredAsTurnOverOrg { get; set; }
    public bool IsRegistered { get; set; }
    public bool IsForeignVendor { get; set; }
}