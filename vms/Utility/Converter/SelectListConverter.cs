using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.viewModels;

namespace vms.Utility.Converter;

public static class SelectListConverter
{
	public static SelectList CustomSelectListToSelectList(this IEnumerable<CustomSelectListItem> items)
	{
		return new SelectList(items, nameof(CustomSelectListItem.Id), nameof(CustomSelectListItem.Name));
	}
}