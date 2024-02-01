using System;

namespace vms.entity.viewModels.NewReports;

public class vmVendorReportRdlc
{
    public int VendorId { get; set; }
    public int? OrganizationId { get; set; }
    public int? VendorOrganizationId { get; set; }
    public string Name { get; set; }
    public string BinNo { get; set; }
    public string NationalIdNo { get; set; }
    public string Address { get; set; }
    public string ContactNo { get; set; }
    public bool IsRegisteredAsTurnOverOrg { get; set; }
    public bool IsRegistered { get; set; }
    public int? CustomsAndVatcommissionarateId { get; set; }
    public bool IsForeignVendor { get; set; }
    public int? CountryId { get; set; }
    public string ReferenceKey { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public long? ApiTransactionId { get; set; }
    public string OrgName { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
}