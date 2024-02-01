using System;

namespace vms.entity.viewModels.NewReports;

public class vmCustomerReportRdlc
{
    public int CustomerId { get; set; }
    public int? OrganizationId { get; set; }
    public int? CustomerOrganizationId { get; set; }
    public string Name { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationAddress { get; set; }
    public string OrganizationBin { get; set; }
    public string Bin { get; set; }
    public string Nidno { get; set; }
    public int? CountryId { get; set; }
    public string Address { get; set; }
    public string PhoneNo { get; set; }
    public string EmailAddress { get; set; }
    public bool IsActive { get; set; }
    public int? CustomsAndVatcommissionarateId { get; set; }
    public bool IsForeignCustomer { get; set; }
    public bool IsFullExportOriented { get; set; }
    public string DeliveryAddress { get; set; }
    public string DeliveryContactPerson { get; set; }
    public string DeliveryContactPersonMobile { get; set; }
    public string ReferenceKey { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedTime { get; set; }
    public long? ApiTransactionId { get; set; }
    public string OrgName { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
}