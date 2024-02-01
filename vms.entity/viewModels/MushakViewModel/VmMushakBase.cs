using vms.entity.Enums;

namespace vms.entity.viewModels.MushakViewModel;

public class VmMushakBase
{
    public EnumLanguage Language { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationAddress { get; set; }
    public string BinNo { get; set; }
    public string BranchName { get; set; }
    public string BranchAddress { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public string HeaderDateString { get; set; }
    public string HeaderProductString { get; set; }
    public string HeaderOtherString { get; set; }
}