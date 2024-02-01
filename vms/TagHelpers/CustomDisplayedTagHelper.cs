using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace vms.TagHelpers
{
	[HtmlTargetElement("div", Attributes = "is-display")]
	public class CustomDisplayedTagHelper : TagHelper
	{
		[HtmlAttributeName("is-display")]
		public bool IsDisplay { get; set; } = false;

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{

			if (!IsDisplay)
			{
				output.AddClass("d-none-custom", HtmlEncoder.Default);
			}

			return base.ProcessAsync(context, output);
		}
	}
}
