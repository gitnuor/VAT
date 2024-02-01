using System.Collections.Generic;

namespace vms.entity.viewModels.VendorViewModel;

public class VendorLocalDetailDisplayViewModel
{
	public VendorLocalDetailViewModel VendorLocal { get; set; }
	public IEnumerable<VendorBranchViewModel> VendorBranches { get; set; }
	public IEnumerable<UploadedContentViewModel> UploadedContents { get; set; }
}