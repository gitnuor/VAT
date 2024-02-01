using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace vms.TagHelpers;

public class GridFreezeMenuTagHelper : TagHelper
{
	public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
	{
		output.TagName = "div";
		output.Attributes.SetAttribute("class", "nav navbar-nav");
		output.Content.SetHtmlContent(@"
<div class='dropdown dropdown-freeze-grid-action'>
	<a href='#' class='' data-bs-toggle='dropdown' data-close-others='true'>
		<i class='text-dark bi bi-three-dots-vertical'></i>
	</a>
	<ul class=' dropdown-menu dropdown-menu-right div-with-arrow-right-top'>
		");
		output.Content.AppendHtml(await output.GetChildContentAsync());
		output.Content.AppendHtml(@"
	</ul>
</div>
		");
	}
}