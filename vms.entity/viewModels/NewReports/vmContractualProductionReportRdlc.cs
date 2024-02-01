using System;

namespace vms.entity.viewModels.NewReports;

public class vmContractualProductionReportRdlc
{
    public int ContractualProductionId { get; set; }
    public int? ContractTypeId { get; set; }
    public int OrganizationId { get; set; }
    public string ChallanNo { get; set; }
    public string ContractNo { get; set; }
    public int? VendorId { get; set; }
    public int? CustomerId { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime ContractDate { get; set; }
    public bool IsClosed { get; set; }
    public DateTime ClosingDate { get; set; }
    public int? ClosedBy { get; set; }
    public string ReferenceKey { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public long? ApiTransactionId { get; set; }
    public string OrgName { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
    public string VendorName { get; set; }
    public string ContractTypeName { get; set; }
}