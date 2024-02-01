using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vms.TagHelpers;

[HtmlTargetElement("input", Attributes = "is-required")]
public class CustomRequiredTagHelper : TagHelper
{
	[HtmlAttributeName("is-required")]
	public bool IsRequired { get; set; } = false;

	[HtmlAttributeName("required-message")]
	public string RequiredMessage { get; set; } = "This field is required!";

	public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
	{

		if (context.AllAttributes["data-val-required"] == null && IsRequired)
		{
			output.Attributes.Add("data-val-required", RequiredMessage);
		}

		return base.ProcessAsync(context, output);
	}
}