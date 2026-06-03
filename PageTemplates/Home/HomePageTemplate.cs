using Kentico.PageBuilder.Web.Mvc.PageTemplates;

[assembly: RegisterPageTemplate(
    identifier: "ProjectXperience.Home_Default",
    name: "Home - Default",
    propertiesType: typeof(PageTemplates.Home.HomePageTemplateProperties),
    customViewName: "~/PageTemplates/Home/_Home.cshtml",
    Description = "Home page template with 4 editable areas for flexible content layout",
    IconClass = "xp-layout")]

namespace PageTemplates.Home
{
    public class HomePageTemplateProperties : IPageTemplateProperties
    {
        // Add configurable properties here if needed
    }
}
