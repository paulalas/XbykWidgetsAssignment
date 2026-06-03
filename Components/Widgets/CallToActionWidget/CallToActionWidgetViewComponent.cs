using System;
using System.Linq;
using System.Threading.Tasks;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.ContentEngine;
using Kentico.Content.Web.Mvc;

namespace Components.Widgets.CallToActionWidget
{
    /// <summary>
    /// Controller for Call to Action widget.
    /// </summary>
    [ViewComponent(Name = "ProjectXperience.CallToActionWidget")]
    public class CallToActionWidgetViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "ProjectXperience.CallToActionWidget";

        private readonly IContentRetriever contentRetriever;

        public CallToActionWidgetViewComponent(IContentRetriever contentRetriever)
        {
            this.contentRetriever = contentRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync(CallToActionWidgetProperties properties)
        {
            // Retrieve the background image from Content Hub if selected
            string imageUrl = null;
            if (properties.Image != null && properties.Image.Any())
            {
                var imageGuid = properties.Image.First().Identifier;

                try
                {
                    // Retrieve the ImageAssets content item from Content Hub
                    var images = await contentRetriever.RetrieveContentByGuids<global::Widgets.ImageAssets>(
                        new[] { imageGuid },
                        new RetrieveContentParameters
                        {
                            LinkedItemsMaxLevel = 1
                        });

                    var image = images.FirstOrDefault();
                    if (image?.image != null)
                    {
                        // Access URL from Content Hub asset
                        var assetUrl = image.image.Url;

                        if (!string.IsNullOrWhiteSpace(assetUrl))
                        {
                            // Handle relative URLs if needed
                            if (assetUrl.StartsWith('~'))
                            {
                                imageUrl = Url.Content(assetUrl);
                            }
                            else
                            {
                                imageUrl = assetUrl;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // If asset retrieval fails, use null (will fall back to default styling)
                    imageUrl = null;
                }
            }

            var viewModel = new CallToActionWidgetViewModel
            {
                Title = properties.Title,
                Description = properties.Description,
                ButtonText = properties.ButtonText,
                ButtonLink = properties.ButtonLink,
                ImageUrl = imageUrl
            };

            return View("~/Components/Widgets/CallToActionWidget/_CallToActionWidget.cshtml", viewModel);
        }
    }
}
