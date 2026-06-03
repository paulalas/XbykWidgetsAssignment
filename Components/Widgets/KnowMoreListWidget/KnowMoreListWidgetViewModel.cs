using System.Collections.Generic;

namespace Components.Widgets.KnowMoreListWidget
{
    /// <summary>
    /// View model for Know More List widget.
    /// </summary>
    public class KnowMoreListWidgetViewModel
    {
        public string SectionBadge { get; set; }
        public string SectionHeading { get; set; }
        public List<KnowMoreItemViewModel> Items { get; set; } = new List<KnowMoreItemViewModel>();
    }

    /// <summary>
    /// View model for individual Know More item.
    /// </summary>
    public class KnowMoreItemViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ButtonName { get; set; }
        public string ButtonLink { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ColorClass { get; set; }
    }
}
