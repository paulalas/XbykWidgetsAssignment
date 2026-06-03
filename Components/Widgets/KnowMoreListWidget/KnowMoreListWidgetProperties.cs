using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Components.Widgets.KnowMoreListWidget
{
    /// <summary>
    /// Know More List widget properties.
    /// </summary>
    public class KnowMoreListWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Section badge/category text.
        /// </summary>
        [TextInputComponent(Order = 0, Label = "Section Badge (e.g., 'About Us')")]
        public string SectionBadge { get; set; } = "About Us";

        /// <summary>
        /// Section heading.
        /// </summary>
        [TextInputComponent(Order = 1, Label = "Section Heading")]
        public string SectionHeading { get; set; } = "Know more about us.";

        /// <summary>
        /// Maximum number of items to display.
        /// </summary>
        [NumberInputComponent(Order = 2, Label = "Maximum Items")]
        public int MaxItems { get; set; } = 3;
    }
}
