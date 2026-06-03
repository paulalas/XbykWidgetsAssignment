using Kentico.PageBuilder.Web.Mvc;
using Components.Widgets.FrequentQuestionsWidget;

[assembly: RegisterWidget(
    identifier: FrequentQuestionsWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(FrequentQuestionsWidgetViewComponent),
    name: "Frequent Questions",
    propertiesType: typeof(FrequentQuestionsWidgetProperties),
    Description = "Displays frequently asked questions in an accordion layout",
    IconClass = "icon-qu-question-answer")]
