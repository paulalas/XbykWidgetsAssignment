using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Components.Widgets.ContentCardsWidget
{
    /// <summary>
    /// Content Cards widget properties.
    /// </summary>
    public class ContentCardsWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Maximum number of cards to display.
        /// </summary>
        [NumberInputComponent(Order = 0, Label = "Maximum Cards")]
        public int MaxCards { get; set; } = 6;
    }
}
