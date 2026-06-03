using System.Collections.Generic;

namespace Components.Widgets.ContentCardsWidget
{
    /// <summary>
    /// View model for Content Cards widget.
    /// </summary>
    public class ContentCardsWidgetViewModel
    {
        public List<ContentCardItemViewModel> Cards { get; set; } = new List<ContentCardItemViewModel>();
    }

    /// <summary>
    /// View model for individual content card item.
    /// </summary>
    public class ContentCardItemViewModel
    {
        public string Subtitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonName { get; set; }
        public string ButtonLink { get; set; }
        public string BackgroundImageUrl { get; set; }
    }
}
