namespace vms.entity.viewModels.VendorViewModel;

public class VendorBranchViewModel
{
	public int VendorBranchId { get; set; }

	public int VendorId { get; set; }

	public string VendorName { get; set; }

	public string VendorBin { get; set; }

	public string VendorNid { get; set; }

	public int OrganizationId { get; set; }

	public int OrgBranchId { get; set; }

	public string BranchName { get; set; }

	public string BranchAddress { get; set; }

	public bool IsActive { get; set; }

	public string IsActiveStatus { get; set; }
}