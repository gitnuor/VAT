using System.Collections.Generic;
using vms.entity.viewModels;

namespace vms.entity.models;

public partial class ProductionReceive : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;
}