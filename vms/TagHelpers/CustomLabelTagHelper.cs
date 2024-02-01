using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vms.TagHelpers;

[HtmlTargetElement("label", Attributes = "asp-for")]
public class CustomLabelTagHelper : TagHelper
{
	public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
	{
		output.AddClass("form-label", HtmlEncoder.Default);
		output.AddClass("complex-form-label", HtmlEncoder.Default);
		return base.ProcessAsync(context, output);
	}
}