using System.Collections.Generic;

namespace vms.entity.viewModels.CustomerViewModel;

public class CustomerLocalDetailDisplayViewModel
{
	public CustomerLocalDetailViewModel CustomerLocal { get; set; }
	public IEnumerable<CustomerBranchViewModel> CustomerBranches { get; set; }
	public IEnumerable<UploadedContentViewModel> UploadedContents { get; set; }
}