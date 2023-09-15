using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NetworkDashboard.classes.TagHelpers
{
    public class DisplayTitleTagHelper : TagHelper
    {
        public ModelExpression aspFor { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator _generator { get; set; }

        public DisplayTitleTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            var propMetadata = aspFor.Metadata;
            var @class = "col-sm-2 col-form-label";

            var label = _generator.GenerateLabel(ViewContext, aspFor.ModelExplorer,
                propMetadata.Name, propMetadata.DisplayName, new { @class });
            
            if (propMetadata.IsRequired)
            {
                var span = new TagBuilder("span");
                span.AddCssClass("text-danger");
                span.InnerHtml.Append("*");

                label.AddCssClass("required");
                label.InnerHtml.AppendHtml(span);
            }

            if (string.IsNullOrEmpty(propMetadata.DescrIPtion) == false)
            {
                var attribute = new TagHelperAttribute("title", propMetadata.DescrIPtion);
            }

            output.Content.AppendHtml(label);
            base.Process(context, output);
        }
    }
}
