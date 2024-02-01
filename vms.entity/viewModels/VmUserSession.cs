using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class VmUserSession
{
    public VmUserSession()
    {
        Rights = new List<Right>();
        BranchIds = new List<int>();
    }
    public int OrganizationId { get; set; }
    public string ProtectedOrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationAddress { get; set; }
    public bool IsProductionCompany { get; set; }
    public bool IsImposeServiceCharge { get; set; }
    public decimal ServiceChargePercent { get; set; }
    public bool IsSaleSimplified { get; set; }
    public string InvoiceNameEng { get; set; }
    public string InvoiceNameBan { get; set; }
    public bool IsRequireSkuNo { get; set; }
    public bool IsRequireSkuId { get; set; }
    public bool IsRequireGoodsId { get; set; }
    public bool IsRequirePartNo { get; set; }
    public string UserImageUrl { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public string PreviousData { get; set; }
    public List<Right> Rights { get; set; }
    public bool IsRequireBranch { get; set; }
    public List<int> BranchIds { get; set; }
}