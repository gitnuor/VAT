namespace vms.entity.viewModels.ReportsViewModel;

public class VmReportBase
{
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