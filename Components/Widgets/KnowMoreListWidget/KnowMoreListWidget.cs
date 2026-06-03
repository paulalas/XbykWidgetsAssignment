using Kentico.PageBuilder.Web.Mvc;
using Components.Widgets.KnowMoreListWidget;

[assembly: RegisterWidget(
    identifier: KnowMoreListWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(KnowMoreListWidgetViewComponent),
    name: "Know More List",
    propertiesType: typeof(KnowMoreListWidgetProperties),
    Description = "Displays a grid of Know More cards with section heading",
    IconClass = "icon-lightbulb")]
