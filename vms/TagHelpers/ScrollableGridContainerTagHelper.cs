using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vms.TagHelpers;

public class ScrollableGridContainerTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "gray-slim-scroll-container");
        output.Content.SetHtmlContent(@"
<div class='table-scroll-container'>
		");
        output.Content.AppendHtml(await output.GetChildContentAsync());
        output.Content.AppendHtml(@"
</div>
		");
    }
}