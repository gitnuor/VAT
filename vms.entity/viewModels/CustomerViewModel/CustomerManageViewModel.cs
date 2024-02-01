using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels.CustomerViewModel;

public class CustomerManageViewModel
{
	public Customer Customer { get; set; }
	public IEnumerable<Content> Contents { get; set; }
}