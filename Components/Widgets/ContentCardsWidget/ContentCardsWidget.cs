using Kentico.PageBuilder.Web.Mvc;
using Components.Widgets.ContentCardsWidget;

[assembly: RegisterWidget(
    identifier: ContentCardsWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(ContentCardsWidgetViewComponent),
    name: "Content Cards",
    propertiesType: typeof(ContentCardsWidgetProperties),
    Description = "Displays content cards in a grid layout with gradient wave design",
    IconClass = "icon-rectangle-a-o")]
