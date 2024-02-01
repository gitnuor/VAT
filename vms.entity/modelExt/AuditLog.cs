using System.Collections.Generic;
using vms.entity.viewModels;


namespace vms.entity.models;

public partial class AuditLog : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;
}