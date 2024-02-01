using System;

namespace vms.entity.viewModels.NewReports;

public class vmUserReportRdlc
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public int UserTypeId { get; set; }
    public int RoleId { get; set; }
    public int? OrganizationId { get; set; }
    public string Mobile { get; set; }
    public string NidNo { get; set; }
    public string TinNo { get; set; }
    public string Address { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public bool IsDefaultPassword { get; set; }
    public bool IsCompanyRepresentative { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string OrgName { get; set; }
	public string OrganizationAddress { get; set; }
	public string OrganizationBin { get; set; }
	public string RoleName { get; set; }
    public string StatusName => IsActive ? "Active" : "Inactive";
}