using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vms.TagHelpers;

[HtmlTargetElement("select", Attributes = "asp-for")]
public class CustomDropdownTagHelper : TagHelper
{

	[HtmlAttributeName("is-searchable")]
	public bool IsSearchable { get; set; } = true;

	public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
	{

		output.AddClass("searchable-dropdown", HtmlEncoder.Default);
		if (context.AllAttributes["data-live-search"] == null && IsSearchable)
		{
			output.Attributes.Add("data-live-search", IsSearchable);
		}
		if (context.AllAttributes["data-style"] == null && IsSearchable)
		{
			output.Attributes.Add("data-style", "bootstrap-select-option");
		}
		return base.ProcessAsync(context, output);
	}


}
