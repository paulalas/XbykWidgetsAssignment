using System.Collections.Generic;

namespace Components.Widgets.FrequentQuestionsWidget
{
    /// <summary>
    /// View model for Frequent Questions widget.
    /// </summary>
    public class FrequentQuestionsWidgetViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<QuestionItemViewModel> Questions { get; set; } = new List<QuestionItemViewModel>();
    }

    /// <summary>
    /// View model for individual question item.
    /// </summary>
    public class QuestionItemViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
