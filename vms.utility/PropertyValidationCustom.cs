using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vms.entity.Enums;
using vms.entity.models;

namespace vms.utility
{
	public static class PropertyValidationCustom
	{
		public static string IsRequiredField(IEnumerable<ViewOrganizationConfigurationBoolean> model, int input)
		{
			return  (model.First(x => x.OrganizationConfigurationBooleanTypeId == input).ConfigurationValue) ? "required" : "";
		}

		public static string IsDisplayedField(IEnumerable<ViewOrganizationConfigurationBoolean> model, int input)
		{
			return (model.First(x => x.OrganizationConfigurationBooleanTypeId == input).ConfigurationValue) ? "" : "d-none-custom";
		}
		
	}
}
