using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.viewModels;

namespace vms.entity.models;

public partial class ContractualProduction : VmsBaseModel
{
	[NotMapped]
	public string VendorDisplayName => Vendor == null ? "" : Vendor.Name;
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;
}