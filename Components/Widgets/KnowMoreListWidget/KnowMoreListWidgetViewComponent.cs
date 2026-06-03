using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.ContentEngine;
using Kentico.Content.Web.Mvc;

namespace Components.Widgets.KnowMoreListWidget
{
    /// <summary>
    /// Controller for Know More List widget.
    /// </summary>
    [ViewComponent(Name = "ProjectXperience.KnowMoreListWidget")]
    public class KnowMoreListWidgetViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "ProjectXperience.KnowMoreListWidget";

        private readonly IContentRetriever contentRetriever;
        private static readonly string[] ColorClasses = new[] 
        { 
            "text-blue-300", 
            "text-purple-300", 
            "text-pink-300",
            "text-green-300",
            "text-yellow-300",
            "text-indigo-300"
        };

        public KnowMoreListWidgetViewComponent(IContentRetriever contentRetriever)
        {
            this.contentRetriever = contentRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync(KnowMoreListWidgetProperties properties)
        {
            var items = new List<KnowMoreItemViewModel>();

            try
            {
                // Retrieve KnowMore pages
                var knowMorePages = await contentRetriever.RetrievePages<global::Widgets.KnowMore>(
                    new RetrievePagesParameters
                    {
                        LinkedItemsMaxLevel = 1,
                        IncludeSecuredItems = false
                    },
                    query => query.TopN(properties.MaxItems),
                    new RetrievalCacheSettings($"KnowMoreList_{properties.MaxItems}", TimeSpan.FromMinutes(5)),
                    HttpContext.RequestAborted
                );

                // Map to view models
                var colorIndex = 0;
                foreach (var page in knowMorePages)
                {
                    // Get thumbnail URL
                    string thumbnailUrl = null;
                    var thumbnail = page.KnowMoreThumbnail?.FirstOrDefault();
                    if (thumbnail?.image != null)
                    {
                        var assetUrl = thumbnail.image.Url;
                        if (!string.IsNullOrWhiteSpace(assetUrl))
                        {
                            thumbnailUrl = assetUrl.StartsWith('~') 
                                ? Url.Content(assetUrl) 
                                : assetUrl;
                        }
                    }

                    items.Add(new KnowMoreItemViewModel
                    {
                        Title = page.KnowMoreTitle ?? "",
                        Description = page.KnowMoreDescription ?? "",
                        ButtonName = page.KnowMoreButtonName ?? "Learn more",
                        ButtonLink = page.KnowMoreButtonLink ?? "#",
                        ThumbnailUrl = thumbnailUrl,
                        ColorClass = ColorClasses[colorIndex % ColorClasses.Length]
                    });

                    colorIndex++;
                }
            }
            catch (Exception)
            {
                // If retrieval fails, return empty list
                items = new List<KnowMoreItemViewModel>();
            }

            var viewModel = new KnowMoreListWidgetViewModel
            {
                SectionBadge = properties.SectionBadge,
                SectionHeading = properties.SectionHeading,
                Items = items
            };

            return View("~/Components/Widgets/KnowMoreListWidget/_KnowMoreListWidget.cshtml", viewModel);
        }
    }
}
