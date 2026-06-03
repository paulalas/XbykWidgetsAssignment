using Kentico.PageBuilder.Web.Mvc;
using Components.Widgets.CallToActionWidget;

[assembly: RegisterWidget(
    identifier: CallToActionWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(CallToActionWidgetViewComponent),
    name: "Call to Action",
    propertiesType: typeof(CallToActionWidgetProperties),
    Description = "A call to action section with title, description, button, and background image",
    IconClass = "icon-megaphone")]
