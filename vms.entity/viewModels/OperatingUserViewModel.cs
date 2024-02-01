using System;

namespace vms.entity.viewModels;

public class OperatingUserViewModel
{
	public int UserId { get; set; }
	public int OrganizationId { get; set; }
	public DateTime OperationTime { get; set; }
}