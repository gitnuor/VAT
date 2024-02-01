using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vms.TagHelpers;

[HtmlTargetElement("input", Attributes = "asp-for")]
[HtmlTargetElement("textarea", Attributes = "asp-for")]
public class CustomInputTextTagHelper : TagHelper
{
	public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var attrs = context.AllAttributes.ToList();
        if (attrs.Any(a => a.Name == "type" && (a.Value.ToString() == "radio" || a.Value.ToString() == "checkbox")))
            return base.ProcessAsync(context, output);

        output.AddClass("form-control", HtmlEncoder.Default);
        output.AddClass("form-control-sm", HtmlEncoder.Default);
        return base.ProcessAsync(context, output);
	}
}
