using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.ContentEngine;
using Kentico.Content.Web.Mvc;

namespace Components.Widgets.ContentCardsWidget
{
    /// <summary>
    /// Controller for Content Cards widget.
    /// </summary>
    [ViewComponent(Name = "ProjectXperience.ContentCardsWidget")]
    public class ContentCardsWidgetViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "ProjectXperience.ContentCardsWidget";

        private readonly IContentRetriever contentRetriever;

        public ContentCardsWidgetViewComponent(IContentRetriever contentRetriever)
        {
            this.contentRetriever = contentRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync(ContentCardsWidgetProperties properties)
        {
            var cards = new List<ContentCardItemViewModel>();

            try
            {
                // Retrieve ContentCards pages
                var contentCardPages = await contentRetriever.RetrievePages<global::Widgets.ContentCards>(
                    new RetrievePagesParameters
                    {
                        LinkedItemsMaxLevel = 1,
                        IncludeSecuredItems = false
                    },
                    query => query.TopN(properties.MaxCards),
                    new RetrievalCacheSettings($"ContentCards_{properties.MaxCards}", TimeSpan.FromMinutes(5)),
                    HttpContext.RequestAborted
                );

                // Map to view models
                foreach (var page in contentCardPages)
                {
                    // Get background image URL
                    string backgroundImageUrl = null;
                    var backgroundImage = page.ContentBackgrounImage?.FirstOrDefault();
                    if (backgroundImage?.image != null)
                    {
                        var assetUrl = backgroundImage.image.Url;
                        if (!string.IsNullOrWhiteSpace(assetUrl))
                        {
                            backgroundImageUrl = assetUrl.StartsWith('~')
                                ? Url.Content(assetUrl)
                                : assetUrl;
                        }
                    }

                    cards.Add(new ContentCardItemViewModel
                    {
                        Subtitle = page.ContentSubTItle ?? "",
                        Title = page.ContentTitle ?? "",
                        Description = page.ContentDescription ?? "",
                        ButtonName = page.ContentButtonName ?? "Learn More",
                        ButtonLink = page.ContentButtonLink ?? "#",
                        BackgroundImageUrl = backgroundImageUrl
                    });
                }
            }
            catch (Exception)
            {
                // If retrieval fails, return empty list
                cards = new List<ContentCardItemViewModel>();
            }

            var viewModel = new ContentCardsWidgetViewModel
            {
                Cards = cards
            };

            return View("~/Components/Widgets/ContentCardsWidget/_ContentCardsWidget.cshtml", viewModel);
        }
    }
}
