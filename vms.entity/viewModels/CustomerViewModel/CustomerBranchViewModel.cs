namespace vms.entity.viewModels.CustomerViewModel;

public class CustomerBranchViewModel
{
	public int CustomerBranchId { get; set; }

	public int CustomerId { get; set; }

	public string CustomerName { get; set; }

	public string CustomerBin { get; set; }

	public string CustomerNid { get; set; }

	public int OrganizationId { get; set; }

	public int OrgBranchId { get; set; }

	public string BranchName { get; set; }

	public string BranchAddress { get; set; }

	public bool IsActive { get; set; }

	public string IsActiveStatus { get; set; }
}