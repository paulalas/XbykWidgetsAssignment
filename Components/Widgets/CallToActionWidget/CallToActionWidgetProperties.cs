using System.Collections.Generic;
using System.Linq;
using CMS.ContentEngine;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Components.Widgets.CallToActionWidget
{
    /// <summary>
    /// Call to Action widget properties.
    /// </summary>
    public class CallToActionWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Badge/category text.
        /// </summary>
        [TextInputComponent(Order = 0, Label = "Badge Text (e.g., 'Design Agency')")]
        public string Title { get; set; } = "Design Agency";

        /// <summary>
        /// Main heading text (use line breaks for multi-line).
        /// </summary>
        [TextAreaComponent(Order = 1, Label = "Main Heading")]
        public string Description { get; set; } = "Dedicated to\nbring your\nideas to life.";

        /// <summary>
        /// Button text.
        /// </summary>
        [TextInputComponent(Order = 2, Label = "Button Text")]
        public string ButtonText { get; set; } = "Get Started";

        /// <summary>
        /// Button link URL.
        /// </summary>
        [TextInputComponent(Order = 3, Label = "Button Link")]
        public string ButtonLink { get; set; } = "#";

        /// <summary>
        /// Background/Feature image from Content Hub.
        /// </summary>
        [ContentItemSelectorComponent(
            global::Widgets.ImageAssets.CONTENT_TYPE_NAME,
            Label = "Image",
            MaximumItems = 1,
            Order = 4)]
        public IEnumerable<ContentItemReference> Image { get; set; } = Enumerable.Empty<ContentItemReference>();
    }
}
