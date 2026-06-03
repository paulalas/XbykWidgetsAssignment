using System.Collections.Generic;
using System.Linq;
using CMS.ContentEngine;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Components.Widgets.FrequentQuestionsWidget
{
    /// <summary>
    /// Frequent Questions widget properties.
    /// </summary>
    public class FrequentQuestionsWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Section title.
        /// </summary>
        [TextInputComponent(Order = 0, Label = "Title")]
        public string Title { get; set; } = "Frequently Asked Questions";

        /// <summary>
        /// Section description.
        /// </summary>
        [TextAreaComponent(Order = 1, Label = "Description")]
        public string Description { get; set; } = "Got questions? We've got answers. If you don't see what you're looking for, reach out to our support team.";

        /// <summary>
        /// Selected questions to display.
        /// </summary>
        [ContentItemSelectorComponent(
            global::Widgets.Questions.CONTENT_TYPE_NAME,
            Label = "Select Questions",
            Order = 2)]
        public IEnumerable<ContentItemReference> SelectedQuestions { get; set; } = Enumerable.Empty<ContentItemReference>();
    }
}
