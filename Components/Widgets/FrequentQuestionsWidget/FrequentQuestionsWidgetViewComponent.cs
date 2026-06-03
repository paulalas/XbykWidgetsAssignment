using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.ContentEngine;
using Kentico.Content.Web.Mvc;

namespace Components.Widgets.FrequentQuestionsWidget
{
    /// <summary>
    /// Controller for Frequent Questions widget.
    /// </summary>
    [ViewComponent(Name = "ProjectXperience.FrequentQuestionsWidget")]
    public class FrequentQuestionsWidgetViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "ProjectXperience.FrequentQuestionsWidget";

        private readonly IContentRetriever contentRetriever;

        public FrequentQuestionsWidgetViewComponent(IContentRetriever contentRetriever)
        {
            this.contentRetriever = contentRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync(FrequentQuestionsWidgetProperties properties)
        {
            var questions = new List<QuestionItemViewModel>();

            try
            {
                if (properties.SelectedQuestions != null && properties.SelectedQuestions.Any())
                {
                    // Get the GUIDs of selected questions
                    var questionGuids = properties.SelectedQuestions
                        .Select(q => q.Identifier)
                        .ToArray();

                    // Retrieve selected Questions content items by GUIDs
                    var questionItems = await contentRetriever.RetrieveContentByGuids<global::Widgets.Questions>(
                        questionGuids,
                        new RetrieveContentParameters
                        {
                            LinkedItemsMaxLevel = 0
                        },
                        HttpContext.RequestAborted
                    );

                    // Create a dictionary for quick lookup
                    var questionDict = questionItems.ToDictionary(q => q.SystemFields.ContentItemGUID);

                    // Map to view models in the same order as selected
                    foreach (var selectedQuestion in properties.SelectedQuestions)
                    {
                        if (questionDict.TryGetValue(selectedQuestion.Identifier, out var item))
                        {
                            questions.Add(new QuestionItemViewModel
                            {
                                Question = item.QuestionTitle ?? "",
                                Answer = item.QuestionDescription ?? ""
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                // If retrieval fails, return empty list
                questions = new List<QuestionItemViewModel>();
            }

            var viewModel = new FrequentQuestionsWidgetViewModel
            {
                Title = properties.Title,
                Description = properties.Description,
                Questions = questions
            };

            return View("~/Components/Widgets/FrequentQuestionsWidget/_FrequentQuestionsWidget.cshtml", viewModel);
        }
    }
}
