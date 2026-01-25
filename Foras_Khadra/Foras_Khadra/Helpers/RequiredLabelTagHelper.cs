using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("label", Attributes = "asp-for")]
public class RequiredLabelTagHelper : TagHelper
{
    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (For != null && For.Metadata.IsRequired)
        {
            // أضف نجمة حمراء بجانب نص الـ Label
            output.Content.AppendHtml(" <span class=\"text-danger\">*</span>");
        }
    }
}
