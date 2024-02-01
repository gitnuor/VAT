using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vms.TagHelpers;

[HtmlTargetElement("span", Attributes = "asp-validation-for")]
public class CustomErrorDisplayedTagHelper : TagHelper
{
	public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
	{
		output.AddClass("text-danger", HtmlEncoder.Default);
		return base.ProcessAsync(context, output);
	}
}