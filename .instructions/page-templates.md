# Creating Page Templates in Xperience by Kentico

This guide provides instructions for creating page templates correctly in Xperience by Kentico projects.

## File Structure

Page templates should be organized in the `~/PageTemplates` folder. **CRITICAL:** You MUST create `_ViewImports.cshtml` and `_ViewStart.cshtml` in BOTH the `~/PageTemplates` folder AND the `~/Views/Shared` folder for Page Builder to work correctly.

```
PageTemplates/
├── _ViewImports.cshtml          (REQUIRED: Kentico namespaces and tag helpers)
├── _ViewStart.cshtml             (REQUIRED: sets default layout)
└── [TemplateName]/
    ├── [TemplateName]PageTemplate.cs    (Registration)
    └── _[TemplateName].cshtml           (View - MUST have underscore prefix)

Views/
└── Shared/
    ├── _ViewImports.cshtml      (REQUIRED: Kentico namespaces and tag helpers)
    ├── _ViewStart.cshtml         (REQUIRED: sets default layout)
    └── _Layout.cshtml            (Site-wide header/footer/chrome)
```

## Key Principle: Separation of Layout and Template

**IMPORTANT:** Page templates should NOT include site-wide header/footer elements. These belong in `~/Views/Shared/_Layout.cshtml`.

- **Layout (`_Layout.cshtml`)** = Site chrome (header, footer, navigation, global styles)
- **Page Template** = Content structure with editable areas for Page Builder content

## Step-by-Step Instructions

### 1. Create the Page Template Registration File

**Location:** `~/PageTemplates/[TemplateName]/[TemplateName]PageTemplate.cs`

**Template:**
```csharp
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

[assembly: RegisterPageTemplate(
    identifier: "ProjectName.TemplateName_Variant",
    name: "Display Name",
    propertiesType: typeof(PageTemplates.TemplateName.TemplateNamePageTemplateProperties),
    customViewName: "~/PageTemplates/TemplateName/_TemplateName.cshtml",
    ContentTypeNames = new[] { ContentType.CONTENT_TYPE_NAME },
    Description = "Description shown in admin",
    IconClass = "xp-layout")]

namespace PageTemplates.TemplateName
{
    public class TemplateNamePageTemplateProperties : IPageTemplateProperties
    {
        // Add configurable properties here if needed
    }
}
```

**✅ DO:**
- Use a **unique namespace** like `PageTemplates.TemplateName` (NOT the same as content types)
- Use `ContentTypeNames` parameter to scope the template to specific content types
- Use icons with `xp-` prefix (e.g., `xp-layout`, `xp-doc`, `xp-grid`)
- Include a unique identifier prefix (e.g., company name, project name)
- Use descriptive variant names if multiple templates exist (e.g., `_Default`, `_TwoColumn`)

**❌ DON'T:**
- Don't use the same namespace as your content types (e.g., `BoilerPage` if that's where your content types are)
- Don't use custom icon classes without the `xp-` prefix
- Don't omit the `customViewName` parameter
- Don't forget to reference the content type constant correctly

### 2. Create the Page Template View

**Location:** `~/PageTemplates/[TemplateName]/_[TemplateName].cshtml`

**CRITICAL: The filename MUST start with an underscore (`_`)**

**RECOMMENDED: Use Layout Approach (with _ViewImports and _ViewStart):**

When you have `_ViewImports.cshtml` and `_ViewStart.cshtml` set up (recommended), your page template is clean and simple:

```cshtml
@model TemplateViewModel<PageTemplates.TemplateName.TemplateNamePageTemplateProperties>

@{
    ViewData["Title"] = "Page Title";
}

<!-- Hero Section -->
<section class="hero-section">
    <editable-area area-identifier="hero" />
</section>

<!-- Main Content -->
<section class="container my-5">
    <div class="row">
        <div class="col-lg-8">
            <editable-area area-identifier="main-content" />
        </div>
        <div class="col-lg-4">
            <editable-area area-identifier="sidebar" />
        </div>
    </div>
</section>
```

**Why this approach is better:**
- ✅ No `@using` directives needed (handled by `_ViewImports.cshtml`)
- ✅ No explicit `Layout` assignment needed (handled by `_ViewStart.cshtml`)
- ✅ Page Builder styles/scripts come from `_Layout.cshtml`
- ✅ Site header/footer automatically included from layout
- ✅ Cleaner, more maintainable code

**Alternative: Self-Contained Approach (NOT recommended):**

If you don't use layouts, you need the full HTML structure:

```cshtml
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder
@using Kentico.Web.Mvc

@model TemplateViewModel<PageTemplates.TemplateName.TemplateNamePageTemplateProperties>

@{
    ViewData["Title"] = "Page Title";
}

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"]</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    
    <!-- REQUIRED: Page Builder styles -->
    <page-builder-styles />
</head>
<body>
    <!-- Main content area with editable sections -->
    <main class="container my-4">
        <editable-area area-identifier="main" />
    </main>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    
    <!-- REQUIRED: Page Builder scripts -->
    <page-builder-scripts />
</body>
</html>
```

**✅ DO:**
- Use `@model TemplateViewModel<YourPropertiesType>` if you defined properties
- Use `@model TemplateViewModel` ONLY if you didn't define properties (set `propertiesType: null`)
- Prefix the filename with underscore: `_TemplateName.cshtml`
- Use `<editable-area area-identifier="main" />` for Page Builder content
- Use layout approach (recommended) instead of self-contained HTML

**❌ DON'T:**
- Don't forget the underscore prefix on the filename
- Don't include `@using` directives when using `_ViewImports.cshtml`
- Don't set `Layout` explicitly when using `_ViewStart.cshtml`
- Don't use `<page-builder-editable-area />` - use `<editable-area />` tag helper
- Don't mismatch the model type (if you register with properties, use `TemplateViewModel<TProperties>`)

### 3. Editable Areas - CORRECT SYNTAX

**✅ CORRECT - Use Tag Helper:**
```cshtml
<editable-area area-identifier="main" />
<editable-area area-identifier="sidebar" />
```

**❌ WRONG - Don't use this:**
```cshtml
<!-- OLD SYNTAX - DON'T USE -->
<page-builder-editable-area area-identifier="main" />
```

**Multiple Editable Areas Example:**
```cshtml
<div class="row">
    <div class="col-md-8">
        <editable-area area-identifier="main" />
    </div>
    <div class="col-md-4">
        <editable-area area-identifier="sidebar" />
    </div>
</div>
```

### 4. Site Layout (_Layout.cshtml)

Header, footer, and navigation should be in `~/Views/Shared/_Layout.cshtml`, NOT in page templates.

**Example _Layout.cshtml:**
```cshtml
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Site Name</title>
    
    <page-builder-styles />
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    <!-- Site Header -->
    <header class="main-header">
        <nav class="navbar navbar-expand-lg">
            <!-- Navigation here -->
        </nav>
    </header>

    <!-- Main Content (from page template) -->
    <main>
        @RenderBody()
    </main>

    <!-- Site Footer -->
    <footer class="main-footer">
        <p>&copy; 2026 Company Name</p>
    </footer>

    <page-builder-scripts />
    <script src="~/js/site.js"></script>
</body>
</html>
```

**✅ Layout Contains:**
- Site-wide header
- Global navigation
- Footer
- Global CSS/JS
- `@RenderBody()` where page template content goes

**✅ Page Template Contains:**
- Page-specific structure
- Editable areas for Page Builder content
- Template-specific layout (e.g., columns, grids)

### 5. Accessing Page Content Data (Optional)

If you need to display content type fields, create a service class:

```csharp
// ~/PageTemplates/TemplateName/TemplateNameService.cs
using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using Kentico.Content.Web.Mvc;
using CMS.Websites;

public interface ITemplateNameService
{
    Task<YourContentType> GetPageDataAsync();
}

public class TemplateNameService : ITemplateNameService
{
    private readonly IWebPageDataContextRetriever _dataRetriever;
    private readonly IContentQueryExecutor _queryExecutor;
    
    public TemplateNameService(
        IWebPageDataContextRetriever dataRetriever,
        IContentQueryExecutor queryExecutor)
    {
        _dataRetriever = dataRetriever;
        _queryExecutor = queryExecutor;
    }
    
    public async Task<YourContentType> GetPageDataAsync()
    {
        var context = _dataRetriever.Retrieve();
        
        var query = new ContentItemQueryBuilder()
            .ForContentType(YourContentType.CONTENT_TYPE_NAME,
                config => config
                    .Where(where => where.WhereEquals(nameof(WebPageFields.WebPageItemID), context.WebPage.WebPageItemID))
                    .TopN(1));
        
        var result = await _queryExecutor.GetMappedWebPageResult<YourContentType>(query);
        return result.FirstOrDefault();
    }
}
```

Register in `Program.cs`:
```csharp
builder.Services.AddScoped<ITemplateNameService, TemplateNameService>();
```

Use in view:
```cshtml
@inject ITemplateNameService TemplateService
@{
    var pageData = await TemplateService.GetPageDataAsync();
}
<h1>@pageData?.Title</h1>
```

### 6. Create _ViewImports.cshtml (REQUIRED)

**CRITICAL:** Without `_ViewImports.cshtml`, Page Builder tag helpers won't work, and you won't see the plus (+) buttons in edit mode.

**You must create this file in TWO locations:**

#### Location 1: `~/PageTemplates/_ViewImports.cshtml`

```cshtml
@using Microsoft.AspNetCore.Mvc
@using Microsoft.AspNetCore.Mvc.Rendering
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder
@using Kentico.Web.Mvc

@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Kentico.Web.Mvc
@addTagHelper *, Kentico.Content.Web.Mvc
```

#### Location 2: `~/Views/Shared/_ViewImports.cshtml`

```cshtml
@using Microsoft.AspNetCore.Mvc
@using Microsoft.AspNetCore.Mvc.Rendering
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder
@using Kentico.Web.Mvc

@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Kentico.Web.Mvc
@addTagHelper *, Kentico.Content.Web.Mvc
```

**What this does:**
- Imports Kentico namespaces so you don't need `@using` in every view
- Registers Page Builder tag helpers: `<editable-area />`, `<page-builder-styles />`, `<page-builder-scripts />`
- Without this, editable areas won't render and Page Builder won't work

### 7. Create _ViewStart.cshtml (REQUIRED)

**CRITICAL:** Without `_ViewStart.cshtml`, your layout won't be applied, and Page Builder scripts/styles won't load.

**You must create this file in TWO locations:**

#### Location 1: `~/PageTemplates/_ViewStart.cshtml`

```cshtml
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
}
```

#### Location 2: `~/Views/Shared/_ViewStart.cshtml`

```cshtml
@{
    Layout = "_Layout";
}
```

**What this does:**
- Automatically applies `_Layout.cshtml` to all page templates
- You don't need to manually set `Layout` in each page template view
- Ensures Page Builder styles and scripts from layout are included

**✅ DO:**
- Create both `_ViewImports.cshtml` and `_ViewStart.cshtml` in BOTH folders
- Use the exact same content in both locations
- Create these files BEFORE creating page templates

**❌ DON'T:**
- Don't skip these files - Page Builder won't work without them
- Don't create only in one location - both are needed
- Don't manually set `Layout` in page template views when using `_ViewStart.cshtml`

### 8. Update _Layout.cshtml for Page Builder

**Location:** `~/Views/Shared/_Layout.cshtml`

Your layout MUST include Page Builder tag helpers and a Scripts section:

```cshtml
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Web.Mvc

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Site Name</title>
    
    <!-- REQUIRED: Page Builder styles MUST be in <head> -->
    <page-builder-styles />
    
    <!-- Your CSS -->
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    <!-- Site Header -->
    <header>
        <!-- Navigation here -->
    </header>

    <!-- Main Content (from page template) -->
    <main>
        @RenderBody()
    </main>

    <!-- Site Footer -->
    <footer>
        <!-- Footer content here -->
    </footer>

    <!-- Your scripts -->
    <script src="~/js/site.js"></script>
    
    <!-- REQUIRED: Page Builder scripts MUST be before </body> -->
    <page-builder-scripts />
    
    <!-- REQUIRED: Scripts section for page templates -->
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

**✅ DO:**
- Include `<page-builder-styles />` in `<head>`
- Include `<page-builder-scripts />` before `</body>`
- Include `@await RenderSectionAsync("Scripts", required: false)` after Page Builder scripts
- Put `<page-builder-scripts />` AFTER your own scripts

**❌ DON'T:**
- Don't forget `<page-builder-styles />` - widgets won't render correctly
- Don't forget `<page-builder-scripts />` - Page Builder edit mode won't work
- Don't forget the Scripts section - page templates may need to inject scripts
- Don't put these tag helpers in widget views (only in layout)

### 9. Register Content Type for Page Builder

Ensure your content type is registered in `Program.cs`:

```csharp
builder.Services.AddKentico(features =>
{
    features.UsePageBuilder(new PageBuilderOptions
    {
        ContentTypeNames = new[]
        {
            YourContentType.CONTENT_TYPE_NAME,
        }
    });
    features.UseWebPageRouting();
});
```

## Common Mistakes to Avoid

### ❌ Namespace Conflicts
**Wrong:**
```csharp
namespace BoilerPage.Home  // Same as content type namespace
{
    public class HomePageTemplateProperties : IPageTemplateProperties { }
}
```
**Error:** `The namespace 'BoilerPage' already contains a definition for 'Home'`

**Right:**
```csharp
namespace PageTemplates.Home  // Separate namespace
{
    public class HomePageTemplateProperties : IPageTemplateProperties { }
}
```

### ❌ Model Type Mismatch
**Wrong:**
```cshtml
@model TemplateViewModel  @* Missing generic type *@
```
**Error:** `The model item passed into the ViewDataDictionary is of type... but this ViewDataDictionary instance requires...`

**Right:**
```cshtml
@model TemplateViewModel<PageTemplates.Home.HomePageTemplateProperties>
```

### ❌ Including Header/Footer in Template
**Wrong:**
```cshtml
<!DOCTYPE html>
<html>
<body>
    <header><!-- Site header --></header>
    <main><editable-area area-identifier="main" /></main>
    <footer><!-- Site footer --></footer>
</body>
</html>
```
**Problem:** Duplicates header/footer when _Layout.cshtml also has them

**Right:**
```cshtml
<!DOCTYPE html>
<html>
<body>
    <main class="container my-4">
        <editable-area area-identifier="main" />
    </main>
</body>
</html>
```

### ❌ Wrong Editable Area Syntax
**Wrong:**
```cshtml
<page-builder-editable-area area-identifier="main" />
```

**Right:**
```cshtml
<editable-area area-identifier="main" />
```

### ❌ Missing Underscore Prefix
**Wrong:** `Home.cshtml`
**Right:** `_Home.cshtml`

### ❌ Missing _ViewImports.cshtml and _ViewStart.cshtml
**Problem:** Page Builder edit mode doesn't show plus (+) buttons, or layout is not applied

**Wrong:** No `_ViewImports.cshtml` or `_ViewStart.cshtml` files

**Right:** Create these files in BOTH locations:

`~/PageTemplates/_ViewImports.cshtml`:
```cshtml
@using Microsoft.AspNetCore.Mvc
@using Microsoft.AspNetCore.Mvc.Rendering
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder
@using Kentico.Web.Mvc

@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Kentico.Web.Mvc
@addTagHelper *, Kentico.Content.Web.Mvc
```

`~/PageTemplates/_ViewStart.cshtml`:
```cshtml
@{
    Layout = "_Layout";
}
```

`~/Views/Shared/_ViewImports.cshtml`:
```cshtml
@using Microsoft.AspNetCore.Mvc
@using Microsoft.AspNetCore.Mvc.Rendering
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder
@using Kentico.Web.Mvc

@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Kentico.Web.Mvc
@addTagHelper *, Kentico.Content.Web.Mvc
```

`~/Views/Shared/_ViewStart.cshtml`:
```cshtml
@{
    Layout = "_Layout";
}
```

**Why this matters:**
- Without `_ViewImports.cshtml`: Tag helpers like `<editable-area />` won't work
- Without `_ViewStart.cshtml`: Layout won't be applied, Page Builder scripts won't load
- Result: No plus (+) buttons in edit mode, broken Page Builder functionality

## Real-World Template Example

Based on the working implementation in this project:

**File:** `~/PageTemplates/Home/_Home.cshtml`
```cshtml
@model TemplateViewModel<PageTemplates.Home.HomePageTemplateProperties>

@{
    ViewData["Title"] = "Home";
}

<!-- Hero Section - Full width editable area -->
<section class="hero-section">
    <editable-area area-identifier="hero" />
</section>

<!-- Main Content Section - Two column layout -->
<section class="container my-5">
    <div class="row g-4">
        <!-- Main Content Column -->
        <div class="col-lg-8">
            <editable-area area-identifier="main-content" />
        </div>
        
        <!-- Sidebar Column -->
        <div class="col-lg-4">
            <editable-area area-identifier="sidebar" />
        </div>
    </div>
</section>

<!-- Footer Content Section - Full width editable area -->
<section class="bg-light py-5">
    <div class="container">
        <editable-area area-identifier="footer-content" />
    </div>
</section>

<style>
    .hero-section {
        min-height: 400px;
    }
</style>
```

**Registration:** `~/PageTemplates/Home/HomePageTemplate.cs`
```csharp
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
```

**Required Files:** `~/PageTemplates/_ViewImports.cshtml`
```cshtml
@using Microsoft.AspNetCore.Mvc
@using Microsoft.AspNetCore.Mvc.Rendering
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder
@using Kentico.Web.Mvc

@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Kentico.Web.Mvc
@addTagHelper *, Kentico.Content.Web.Mvc
```

**Required Files:** `~/PageTemplates/_ViewStart.cshtml`
```cshtml
@{
    Layout = "_Layout";
}
```

**Required Files:** `~/Views/Shared/_ViewImports.cshtml`
```cshtml
@using Microsoft.AspNetCore.Mvc
@using Microsoft.AspNetCore.Mvc.Rendering
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder
@using Kentico.Web.Mvc

@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Kentico.Web.Mvc
@addTagHelper *, Kentico.Content.Web.Mvc
```

**Required Files:** `~/Views/Shared/_ViewStart.cshtml`
```cshtml
@{
    Layout = "_Layout";
}
```

## Testing Checklist

**Before Testing:**
- [ ] `~/PageTemplates/_ViewImports.cshtml` exists with Kentico tag helpers
- [ ] `~/PageTemplates/_ViewStart.cshtml` exists with Layout assignment
- [ ] `~/Views/Shared/_ViewImports.cshtml` exists with Kentico tag helpers
- [ ] `~/Views/Shared/_ViewStart.cshtml` exists with Layout assignment
- [ ] `~/Views/Shared/_Layout.cshtml` includes `<page-builder-styles />` in `<head>`
- [ ] `~/Views/Shared/_Layout.cshtml` includes `<page-builder-scripts />` before `</body>`
- [ ] `~/Views/Shared/_Layout.cshtml` includes `@await RenderSectionAsync("Scripts", required: false)`

**After Building:**
- [ ] Build succeeds without errors
- [ ] No namespace conflicts
- [ ] Page template appears in admin UI when creating/editing pages
- [ ] Template is scoped to correct content type(s)
- [ ] Template filename has underscore prefix

**In Page Builder Edit Mode:**
- [ ] Editable areas show plus (+) buttons in edit mode
- [ ] Can add widgets to editable areas
- [ ] Widget configuration dialogs open correctly
- [ ] Page renders correctly in edit mode
- [ ] Page renders correctly in preview/live mode
- [ ] No duplicate headers/footers from layout
- [ ] No console errors in browser

## Troubleshooting Common Issues

### ❌ Problem: Plus (+) Signs Not Showing in Page Builder Edit Mode

**Symptoms:**
- Editable areas appear as empty boxes
- No "+" button to add widgets
- Page Builder edit mode loads but can't add content

**Root Cause:** Missing `_ViewImports.cshtml` and/or `_ViewStart.cshtml` files

**Solution:**

1. **Create `~/PageTemplates/_ViewImports.cshtml`:**
   ```cshtml
   @using Microsoft.AspNetCore.Mvc
   @using Microsoft.AspNetCore.Mvc.Rendering
   @using Kentico.PageBuilder.Web.Mvc
   @using Kentico.Content.Web.Mvc
   @using Kentico.Content.Web.Mvc.PageBuilder
   @using Kentico.Web.Mvc

   @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
   @addTagHelper *, Kentico.Web.Mvc
   @addTagHelper *, Kentico.Content.Web.Mvc
   ```

2. **Create `~/PageTemplates/_ViewStart.cshtml`:**
   ```cshtml
   @{
       Layout = "_Layout";
   }
   ```

3. **Create `~/Views/Shared/_ViewImports.cshtml`:**
   ```cshtml
   @using Microsoft.AspNetCore.Mvc
   @using Microsoft.AspNetCore.Mvc.Rendering
   @using Kentico.PageBuilder.Web.Mvc
   @using Kentico.Content.Web.Mvc
   @using Kentico.Content.Web.Mvc.PageBuilder
   @using Kentico.Web.Mvc

   @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
   @addTagHelper *, Kentico.Web.Mvc
   @addTagHelper *, Kentico.Content.Web.Mvc
   ```

4. **Create `~/Views/Shared/_ViewStart.cshtml`:**
   ```cshtml
   @{
       Layout = "_Layout";
   }
   ```

5. **Verify `~/Views/Shared/_Layout.cshtml` has:**
   ```cshtml
   <head>
       <page-builder-styles />
   </head>
   <body>
       <page-builder-scripts />
       @await RenderSectionAsync("Scripts", required: false)
   </body>
   ```

6. **Rebuild your project**

7. **Refresh the browser** (hard refresh: Ctrl+Shift+R)

### ❌ Problem: Page Template Not Appearing in Admin

**Possible Causes:**
- Content type not registered for Page Builder
- Namespace conflicts
- Build errors

**Solution:**

1. **Check `Program.cs` has Page Builder registration:**
   ```csharp
   builder.Services.AddKentico(features =>
   {
       features.UsePageBuilder();
       features.UseWebPageRouting();
   });
   ```

2. **Check for build errors:**
   - Open Error List in Visual Studio
   - Fix any compilation errors
   - Rebuild solution

3. **Verify namespace is unique:**
   ```csharp
   namespace PageTemplates.Home  // Good - unique namespace
   {
       public class HomePageTemplateProperties : IPageTemplateProperties { }
   }
   ```

### ❌ Problem: Layout Not Applied / Duplicate Headers

**Symptoms:**
- Site header/footer appear twice
- Page Builder styles not loading
- Layout not being applied

**Solution:**

1. **Ensure `_ViewStart.cshtml` exists** in BOTH `~/PageTemplates/` and `~/Views/Shared/`

2. **Remove explicit Layout assignment from page template:**
   ```cshtml
   @* WRONG - Don't do this when using _ViewStart *@
   @{
       Layout = "~/Views/Shared/_Layout.cshtml";
   }
   
   @* RIGHT - Let _ViewStart handle it *@
   @{
       ViewData["Title"] = "Home";
   }
   ```

3. **Verify `_ViewStart.cshtml` content:**
   ```cshtml
   @{
       Layout = "_Layout";
   }
   ```

## Reference Documentation

- [Page Templates for Page Builder](https://docs.kentico.com/documentation/developers-and-admins/development/builders/page-builder/page-templates-for-page-builder)
- [Page Template Properties](https://docs.kentico.com/documentation/developers-and-admins/development/builders/page-builder/page-templates-for-page-builder/page-template-properties)
- [Content Retrieval](https://docs.kentico.com/documentation/developers-and-admins/development/content-retrieval/retrieve-page-content)

---

# Creating Widgets for Page Builder

This guide provides instructions for creating custom widgets that can be used in editable areas within page templates.

## File Structure

Widgets should be organized in the `~/Components/Widgets` folder:

```
Components/
└── Widgets/
    └── [WidgetName]Widget/
        ├── [WidgetName]Widget.cs              (Registration)
        ├── [WidgetName]WidgetProperties.cs    (Properties model)
        └── _[WidgetName]Widget.cshtml         (View - MUST have underscore prefix)
```

## Step-by-Step Instructions

### 1. Create the Widget Properties Class

**Location:** `~/Components/Widgets/[WidgetName]Widget/[WidgetName]WidgetProperties.cs`

This class defines the configurable properties that content editors can set in the widget's configuration dialog.

**Template:**
```csharp
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.PageBuilder.Web.Mvc;

namespace Components.Widgets.WidgetNameWidget
{
    public class WidgetNameWidgetProperties : IWidgetProperties
    {
        // Text input
        [TextInputComponent(Order = 0, Label = "Text")]
        public string Text { get; set; } = "Default value";

        // Multi-line text area
        [TextAreaComponent(Order = 1, Label = "Description")]
        public string Description { get; set; } = "";

        // Number input
        [NumberInputComponent(Order = 2, Label = "Count")]
        public int Count { get; set; } = 5;

        // Dropdown selector
        [DropDownComponent(
            Label = "Style", 
            Order = 3,
            Options = "primary;Primary\r\nsecondary;Secondary\r\nsuccess;Success")]
        public string Style { get; set; } = "primary";

        // URL/Link input
        [TextInputComponent(Order = 4, Label = "Link URL")]
        public string LinkUrl { get; set; } = "#";

        // Checkbox
        [CheckBoxComponent(Label = "Show border", Order = 5)]
        public bool ShowBorder { get; set; } = true;
    }
}
```

**Available Form Components:**

| Component | Attribute | Data Type | Description |
|-----------|-----------|-----------|-------------|
| Text Input | `[TextInputComponent]` | `string` | Single-line text field |
| Text Area | `[TextAreaComponent]` | `string` | Multi-line text field |
| Number Input | `[NumberInputComponent]` | `int?` | Integer number input |
| Decimal Input | `[DecimalNumberInputComponent]` | `decimal?` | Decimal number input |
| Checkbox | `[CheckBoxComponent]` | `bool` | Yes/No checkbox |
| Dropdown | `[DropDownComponent]` | `string` | Single-selection dropdown |
| Rich Text Editor | `[RichTextEditorComponent]` | `string` | HTML editor |
| Date Input | `[DateInputComponent]` | `DateTime?` | Date picker |
| Date/Time Input | `[DateTimeInputComponent]` | `DateTime?` | Date and time picker |

**✅ DO:**
- Use `Kentico.Xperience.Admin.Base.FormAnnotations` namespace for form component attributes
- Set `Order` property to control the display sequence in the configuration dialog
- Provide meaningful `Label` text for each property
- Set sensible default values
- Implement `IWidgetProperties` interface

**❌ DON'T:**
- Don't use `Kentico.Forms.Web.Mvc` namespace (that's for older versions)
- Don't forget the `Order` parameter - it controls field ordering in the admin UI
- Don't use spaces or special characters in property names

### 2. Create the Widget View

**Location:** `~/Components/Widgets/[WidgetName]Widget/_[WidgetName]Widget.cshtml`

**CRITICAL: The filename MUST start with an underscore (`_`)**

**Template:**
```cshtml
@using Kentico.PageBuilder.Web.Mvc
@using Components.Widgets.WidgetNameWidget

@model ComponentViewModel<WidgetNameWidgetProperties>

<div class="widget-container @(Model.Properties.ShowBorder ? "border" : "")">
    @if (!string.IsNullOrWhiteSpace(Model.Properties.Text))
    {
        <h3>@Model.Properties.Text</h3>
    }
    
    @if (!string.IsNullOrWhiteSpace(Model.Properties.Description))
    {
        <p>@Model.Properties.Description</p>
    }
    
    <a href="@Model.Properties.LinkUrl" class="btn btn-@Model.Properties.Style">
        Learn More
    </a>
</div>
```

**✅ DO:**
- Use `@model ComponentViewModel<YourPropertiesType>`
- Access properties via `Model.Properties.PropertyName`
- Check for null/empty values before rendering
- Add custom CSS classes for styling
- Use conditional rendering based on property values

**❌ DON'T:**
- Don't include `<page-builder-styles />` or `<page-builder-scripts />` in widget views
- Don't forget the underscore prefix on the filename
- Don't directly output user input without null checks

### 3. Register the Widget

**Location:** `~/Components/Widgets/[WidgetName]Widget/[WidgetName]Widget.cs`

**Template:**
```csharp
using Kentico.PageBuilder.Web.Mvc;
using Components.Widgets.WidgetNameWidget;

[assembly: RegisterWidget(
    identifier: "CompanyName.WidgetName",
    name: "Widget Display Name",
    propertiesType: typeof(WidgetNameWidgetProperties),
    customViewName: "~/Components/Widgets/WidgetNameWidget/_WidgetNameWidget.cshtml",
    Description = "Description shown in admin UI",
    IconClass = "icon-badge")]
```

**Registration Parameters:**

| Parameter | Required | Description |
|-----------|----------|-------------|
| `identifier` | ✅ Yes | Unique identifier (e.g., "ProjectName.WidgetName") |
| `name` | ✅ Yes | Display name shown in admin UI |
| `propertiesType` | ✅ Yes | Type of the properties class |
| `customViewName` | ✅ Yes | Path to the widget view file |
| `Description` | ⚪ Optional | Help text shown in admin UI |
| `IconClass` | ⚪ Optional | CSS icon class (default: "icon-badge") |

**Common Icon Classes:**
- `icon-badge` - Badge icon
- `icon-picture` - Image icon
- `icon-rectangle-o` - Box/container icon
- `icon-square` - Square icon
- `icon-slider` - Slider icon
- `icon-doc-text` - Document icon

**✅ DO:**
- Use unique identifiers with company/project prefix
- Provide descriptive names and descriptions
- Use the full path for `customViewName`
- Choose appropriate icon classes

**❌ DON'T:**
- Don't use generic identifiers without prefixes
- Don't forget the tilde (~) in the view path
- Don't omit required parameters

## Real-World Widget Example: Hero Banner

Based on the working implementation in this project.

### Properties Class
**File:** `~/Components/Widgets/HeroBannerWidget/HeroBannerWidgetProperties.cs`

```csharp
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.PageBuilder.Web.Mvc;

namespace Components.Widgets.HeroBannerWidget
{
    public class HeroBannerWidgetProperties : IWidgetProperties
    {
        // Background image URL for the hero banner
        [TextInputComponent(Order = 0, Label = "Background image URL (leave empty for default gradient)")]
        public string BackgroundImageUrl { get; set; } = "";

        // First title - "Welcome to UniNews"
        [TextInputComponent(Order = 1, Label = "Welcome text (small text)")]
        public string WelcomeText { get; set; } = "Welcome to UniNews";

        // Second title - "Your Campus. Your Stories."
        [TextAreaComponent(Order = 2, Label = "Main heading")]
        public string MainHeading { get; set; } = "Your Campus.\nYour Stories.";

        // Description text
        [TextAreaComponent(Order = 3, Label = "Description")]
        public string Description { get; set; } = "Stay informed with the latest news, events, and updates from across the university community.";

        // First button - Explore News
        [TextInputComponent(Order = 4, Label = "First button text")]
        public string FirstButtonText { get; set; } = "Explore News";

        [TextInputComponent(Order = 5, Label = "First button URL")]
        public string FirstButtonUrl { get; set; } = "/news";

        // Second button - About Us
        [TextInputComponent(Order = 6, Label = "Second button text")]
        public string SecondButtonText { get; set; } = "About Us";

        [TextInputComponent(Order = 7, Label = "Second button URL")]
        public string SecondButtonUrl { get; set; } = "/about";
    }
}
```

### Widget View
**File:** `~/Components/Widgets/HeroBannerWidget/_HeroBannerWidget.cshtml`

```cshtml
@using Kentico.PageBuilder.Web.Mvc
@using Components.Widgets.HeroBannerWidget

@model ComponentViewModel<HeroBannerWidgetProperties>

@{
    var backgroundStyle = !string.IsNullOrWhiteSpace(Model.Properties.BackgroundImageUrl)
        ? $"background-image: url('{Model.Properties.BackgroundImageUrl}'); background-size: cover; background-position: center;" 
        : "background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);";
}

<style>
    .hero-banner-widget {
        position: relative;
        min-height: 500px;
        background-size: cover;
        background-position: center;
        display: flex;
        align-items: center;
        justify-content: center;
        overflow: hidden;
    }
    
    .hero-overlay {
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0,0,0,0.3);
    }
    
    .hero-content {
        position: relative;
        z-index: 1;
        padding: 4rem 1rem;
    }
</style>

<section class="hero-banner-widget" style="@backgroundStyle">
    <div class="hero-overlay"></div>
    <div class="container hero-content text-center text-white">
        @if (!string.IsNullOrWhiteSpace(Model.Properties.WelcomeText))
        {
            <p class="text-uppercase fw-semibold mb-2" style="letter-spacing:.15em;font-size:.82rem;color:rgba(255,200,100,.85);">
                <i class="bi bi-broadcast me-1"></i>@Model.Properties.WelcomeText
            </p>
        }
        
        @if (!string.IsNullOrWhiteSpace(Model.Properties.MainHeading))
        {
            <h1 class="fw-bold mb-3">
                @foreach (var line in Model.Properties.MainHeading.Split('\n'))
                {
                    @line<br class="d-none d-sm-block" />
                }
            </h1>
        }
        
        @if (!string.IsNullOrWhiteSpace(Model.Properties.Description))
        {
            <p class="lead mb-4 mx-auto">
                @foreach (var line in Model.Properties.Description.Split('\n'))
                {
                    @line<br class="d-none d-md-block" />
                }
            </p>
        }
        
        <div class="d-flex gap-3 justify-content-center flex-wrap">
            @if (!string.IsNullOrWhiteSpace(Model.Properties.FirstButtonText))
            {
                <a href="@Model.Properties.FirstButtonUrl" class="btn btn-danger btn-lg px-4 fw-semibold">
                    <i class="bi bi-newspaper me-2"></i>@Model.Properties.FirstButtonText
                </a>
            }
            
            @if (!string.IsNullOrWhiteSpace(Model.Properties.SecondButtonText))
            {
                <a href="@Model.Properties.SecondButtonUrl" class="btn btn-outline-light btn-lg px-4">
                    @Model.Properties.SecondButtonText
                </a>
            }
        </div>
    </div>
</section>
```

### Widget Registration
**File:** `~/Components/Widgets/HeroBannerWidget/HeroBannerWidget.cs`

```csharp
using Kentico.PageBuilder.Web.Mvc;
using Components.Widgets.HeroBannerWidget;

[assembly: RegisterWidget(
    identifier: "UniNews.HeroBannerWidget",
    name: "Hero Banner",
    propertiesType: typeof(HeroBannerWidgetProperties),
    customViewName: "~/Components/Widgets/HeroBannerWidget/_HeroBannerWidget.cshtml",
    Description = "A hero banner section with customizable background, headings, and call-to-action buttons",
    IconClass = "icon-badge")]
```

## Common Widget Development Patterns

### Conditional Rendering
```cshtml
@if (!string.IsNullOrWhiteSpace(Model.Properties.Title))
{
    <h2>@Model.Properties.Title</h2>
}
```

### Dynamic CSS Classes
```cshtml
<div class="widget @(Model.Properties.FullWidth ? "container-fluid" : "container")">
    <!-- Content -->
</div>
```

### Multi-line Text with Line Breaks
```cshtml
@foreach (var line in Model.Properties.Description.Split('\n'))
{
    @line<br />
}
```

### Dynamic Background Images
```cshtml
@{
    var backgroundStyle = !string.IsNullOrWhiteSpace(Model.Properties.ImageUrl)
        ? $"background-image: url('{Model.Properties.ImageUrl}');"
        : "background-color: #f0f0f0;";
}
<div style="@backgroundStyle">
    <!-- Content -->
</div>
```

### Bootstrap Button Classes from Property
```cshtml
<a href="@Model.Properties.LinkUrl" class="btn btn-@Model.Properties.ButtonStyle">
    @Model.Properties.ButtonText
</a>
```

## Using Widgets in Page Templates

To use widgets in page templates, add editable areas:

```cshtml
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder

@model TemplateViewModel<PageTemplates.Home.HomePageTemplateProperties>

<!-- Hero Banner Area - Users can add Hero Banner widget here -->
<editable-area area-identifier="hero-banner" />

<!-- Main Content Area - Users can add any widgets here -->
<section class="container py-5">
    <editable-area area-identifier="main-content" />
</section>

<!-- Sidebar Area - For additional widgets -->
<aside class="container py-5">
    <editable-area area-identifier="sidebar" />
</aside>
```

Content editors can then:
1. Open the page in Page Builder
2. Click the "+" button in an editable area
3. Select your custom widget from the widget selector
4. Configure the widget properties in the dialog
5. Save and publish the page

## Testing Checklist

- [ ] Widget appears in Page Builder widget selector
- [ ] All properties are editable in configuration dialog
- [ ] Property values are reflected in the widget output
- [ ] Widget renders correctly on the live site
- [ ] Default values are applied when widget is first added
- [ ] Build succeeds without errors
- [ ] Widget view file has underscore prefix
- [ ] Null/empty value checks prevent errors
- [ ] Custom styles don't conflict with global styles

## Common Widget Mistakes to Avoid

### ❌ Wrong Namespace for Form Components
**Wrong:**
```csharp
using Kentico.Forms.Web.Mvc;  // Old namespace

[MediaFilesSelector(...)]  // Won't work in Xperience by Kentico
```

**Right:**
```csharp
using Kentico.Xperience.Admin.Base.FormAnnotations;

[TextInputComponent(Order = 0, Label = "Image URL")]
public string ImageUrl { get; set; }
```

### ❌ Missing Null Checks
**Wrong:**
```cshtml
<h2>@Model.Properties.Title</h2>  @* Could render empty heading *@
```

**Right:**
```cshtml
@if (!string.IsNullOrWhiteSpace(Model.Properties.Title))
{
    <h2>@Model.Properties.Title</h2>
}
```

---

# Integrating Content Hub Image Assets in Widgets

This guide shows how to properly implement Content Hub asset selection and retrieval in widgets using ViewComponents.

## Overview

Content Hub assets allow editors to upload and manage images/media in Kentico's Content Hub, then select them in widgets through a user-friendly interface. This requires:

1. **Content Type** - A reusable content type with `ContentItemAsset` field
2. **Widget Properties** - Using `ContentItemSelectorComponent` for asset selection
3. **ViewComponent** - Retrieving selected assets using `IContentRetriever`
4. **View Model** - Passing asset URL to the view
5. **View** - Displaying the asset (e.g., background image)

## Step-by-Step Implementation

### 1. Create Asset Content Type (Content Hub)

In Kentico Admin:

1. Navigate to **Content types** → **Reusable content types**
2. Create a new content type (e.g., "Asset")
   - **Code name**: `Uninews.asset` (or your namespace)
   - **Display name**: Asset
3. Add a field:
   - **Field name**: Asset
   - **Field type**: **Content item asset** (NOT Media library - that's deprecated)
   - **Settings**: Configure allowed file types (images only if needed)
4. **Save** and **Generate code files** (regenerate code to get the C# class)

**Generated Code Example:**
```csharp
// ~/ReusableContentTypes/Uninews/Asset/Asset.generated.cs
namespace Uninews
{
    [RegisterContentTypeMapping(CONTENT_TYPE_NAME)]
    public partial class Asset : IContentItemFieldsSource
    {
        public const string CONTENT_TYPE_NAME = "Uninews.asset";
        
        [SystemField]
        public ContentItemFields SystemFields { get; set; }
        
        // Content Hub asset field
        [DatabaseField("Asset")]
        public ContentItemAsset Asset1 { get; set; }
    }
}
```

### 2. Widget Properties - Asset Selector

Use `ContentItemSelectorComponent` to let editors select assets from Content Hub.

**File:** `~/Components/Widgets/HeroBannerWidget/HeroBannerWidgetProperties.cs`

```csharp
using System.Collections.Generic;
using System.Linq;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using Kentico.PageBuilder.Web.Mvc;
using CMS.ContentEngine;

namespace Components.Widgets.HeroBannerWidget
{
    public class HeroBannerWidgetProperties : IWidgetProperties
    {
        // Content Hub asset selector
        [ContentItemSelectorComponent(
            Uninews.Asset.CONTENT_TYPE_NAME,  // References your asset content type
            Label = "Background image (leave empty for default gradient)",
            MaximumItems = 1,
            Order = 0)]
        public IEnumerable<ContentItemReference> BackgroundImage { get; set; } = Enumerable.Empty<ContentItemReference>();
        
        [TextInputComponent(Order = 1, Label = "Welcome text")]
        public string WelcomeText { get; set; } = "Welcome to UniNews";
        
        [TextAreaComponent(Order = 2, Label = "Main heading")]
        public string MainHeading { get; set; } = "Your Campus.\nYour Stories.";
        
        // ... other properties
    }
}
```

**Key Points:**
- Use `ContentItemSelectorComponent` attribute
- First parameter is the content type name (use generated constant)
- Return type must be `IEnumerable<ContentItemReference>`
- Set `MaximumItems = 1` for single selection
- Initialize with `Enumerable.Empty<ContentItemReference>()`

### 3. ViewComponent - Retrieve Asset Data

Create a ViewComponent to retrieve the selected asset and extract its URL.

**File:** `~/Components/Widgets/HeroBannerWidget/HeroBannerWidgetViewComponent.cs`

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.ContentEngine;
using Kentico.Content.Web.Mvc;

namespace Components.Widgets.HeroBannerWidget
{
    [ViewComponent(Name = "UniNews.HeroBannerWidget")]
    public class HeroBannerWidgetViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "UniNews.HeroBannerWidget";
        
        private readonly IContentRetriever contentRetriever;

        public HeroBannerWidgetViewComponent(IContentRetriever contentRetriever)
        {
            this.contentRetriever = contentRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            ComponentViewModel<HeroBannerWidgetProperties> componentViewModel)
        {
            var properties = componentViewModel.Properties;
            
            // Retrieve the background image from Content Hub if selected
            string? backgroundImageUrl = null;
            if (properties.BackgroundImage != null && properties.BackgroundImage.Any())
            {
                var imageGuid = properties.BackgroundImage.First().Identifier;
                
                try
                {
                    // Retrieve the Asset content item from Content Hub
                    var assets = await contentRetriever.RetrieveContentByGuids<Uninews.Asset>(
                        new[] { imageGuid },
                        new RetrieveContentParameters
                        {
                            LinkedItemsMaxLevel = 1
                        });
                    
                    var asset = assets.FirstOrDefault();
                    if (asset?.Asset1 != null)
                    {
                        // Direct access to Content Hub asset URL
                        var assetUrl = asset.Asset1.Url;
                        
                        // Handle relative URLs if needed
                        if (!string.IsNullOrWhiteSpace(assetUrl))
                        {
                            if (assetUrl.StartsWith("~"))
                            {
                                backgroundImageUrl = Url.Content(assetUrl);
                            }
                            else
                            {
                                backgroundImageUrl = assetUrl;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // If Asset retrieval fails, use fallback (null = gradient)
                    backgroundImageUrl = null;
                }
            }

            var viewModel = new HeroBannerWidgetViewModel
            {
                BackgroundImageUrl = backgroundImageUrl,
                WelcomeText = properties.WelcomeText,
                MainHeading = properties.MainHeading,
                Description = properties.Description,
                FirstButtonText = properties.FirstButtonText,
                FirstButtonUrl = properties.FirstButtonUrl,
                SecondButtonText = properties.SecondButtonText,
                SecondButtonUrl = properties.SecondButtonUrl
            };

            return View("~/Components/Widgets/HeroBannerWidget/_HeroBannerWidget.cshtml", viewModel);
        }
    }
}
```

**Key Points:**
- Inject `IContentRetriever` via constructor
- Extract GUID from `ContentItemReference.Identifier`
- Use `RetrieveContentByGuids<T>` for single content type retrieval
- Set `LinkedItemsMaxLevel = 1` to retrieve linked assets
- Access URL via `asset.Asset1.Url` (direct property access)
- Handle `null` gracefully (fallback to default)

### 4. View Model

Simple DTO to pass data to the view.

**File:** `~/Components/Widgets/HeroBannerWidget/HeroBannerWidgetViewModel.cs`

```csharp
namespace Components.Widgets.HeroBannerWidget
{
    public class HeroBannerWidgetViewModel
    {
        public string? BackgroundImageUrl { get; set; }
        public string WelcomeText { get; set; } = "";
        public string MainHeading { get; set; } = "";
        public string Description { get; set; } = "";
        public string FirstButtonText { get; set; } = "";
        public string FirstButtonUrl { get; set; } = "#";
        public string SecondButtonText { get; set; } = "";
        public string SecondButtonUrl { get; set; } = "#";
    }
}
```

### 5. View - Display the Asset

Use the asset URL in your view (e.g., as a background image).

**File:** `~/Components/Widgets/HeroBannerWidget/_HeroBannerWidget.cshtml`

```cshtml
@using Components.Widgets.HeroBannerWidget

@model HeroBannerWidgetViewModel

@{
    var backgroundStyle = !string.IsNullOrWhiteSpace(Model.BackgroundImageUrl)
        ? $"background-image: url('{Model.BackgroundImageUrl}'); background-size: cover; background-position: center;" 
        : "background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);";
}

<section class="hero-banner-widget" style="@backgroundStyle">
    <div class="hero-overlay"></div>
    <div class="container hero-content text-center text-white">
        @if (!string.IsNullOrWhiteSpace(Model.WelcomeText))
        {
            <p class="text-uppercase fw-semibold mb-2">
                <i class="bi bi-broadcast me-1"></i>@Model.WelcomeText
            </p>
        }
        
        @if (!string.IsNullOrWhiteSpace(Model.MainHeading))
        {
            <h1 class="fw-bold mb-3">@Model.MainHeading</h1>
        }
        
        @if (!string.IsNullOrWhiteSpace(Model.Description))
        {
            <p class="lead mb-4">@Model.Description</p>
        }
        
        <div class="d-flex gap-3 justify-content-center">
            @if (!string.IsNullOrWhiteSpace(Model.FirstButtonText))
            {
                <a href="@Model.FirstButtonUrl" class="btn btn-danger btn-lg">
                    @Model.FirstButtonText
                </a>
            }
            
            @if (!string.IsNullOrWhiteSpace(Model.SecondButtonText))
            {
                <a href="@Model.SecondButtonUrl" class="btn btn-outline-light btn-lg">
                    @Model.SecondButtonText
                </a>
            }
        </div>
    </div>
</section>
```

### 6. Widget Registration (ViewComponent Pattern)

**File:** `~/Components/Widgets/HeroBannerWidget/HeroBannerWidget.cs`

```csharp
using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget(
    identifier: HeroBannerWidgetViewComponent.IDENTIFIER,
    viewComponentType: typeof(HeroBannerWidgetViewComponent),
    name: "Hero Banner",
    propertiesType: typeof(HeroBannerWidgetProperties),
    Description = "Hero banner with background image from Content Hub, heading, and CTA buttons",
    IconClass = "icon-picture")]
```

**✅ Key Difference: Widgets vs Page Templates**
- **Widgets**: Use `RetrieveContentByGuids<T>` with `ContentItemReference` GUIDs from properties
- **Page Templates**: Use `RetrievePages<T>` to get the current page's data (see next section)

---

# Retrieving Page Content Data with Content Hub Images in Page Templates

This guide shows how to display photos/assets stored in **Page content types** (not reusable content selected by editors). This is different from widgets because you're retrieving the current page's own data, not selecting content from a library.

## When to Use This Pattern

Use this pattern when:
- **Page content type** has a Content Hub asset field (e.g., `News_Item` with `Profile` image)
- You want to display the page's own data (title, author, images, etc.) in the page template
- Images are part of the page content, NOT selected by editors in widgets/Page Builder

## Overview

To retrieve and display photos from page content types with Content Hub assets:

1. **Page Content Type** - Has a linked reusable content type field (e.g., `IEnumerable<Asset>`)
2. **Page Template View** - Injects `IContentRetriever` and `IWebPageDataContextRetriever`
3. **Retrieve Page Data** - Use `RetrievePages<T>` with `LinkedItemsMaxLevel = 1`
4. **Access Asset URL** - Direct property access: `page.FieldName.FirstOrDefault().Asset1.Url`

## Step-by-Step Implementation

### 1. Create Page Content Type with Asset Field

In Kentico Admin:

1. Navigate to **Content types** → **Page content types**
2. Edit or create a page content type (e.g., "News_Item")
3. Add a field:
   - **Field name**: Profile (or HeroImage, FeaturedImage, etc.)
   - **Field type**: **Reusable content** → Select your Asset content type
   - **Settings**: 
     - Allow multiple items: Yes (for `IEnumerable<Asset>`) or No (for single `Asset`)
     - Maximum items: 1 (if single image)
4. **Save** and **Generate code files**

**Generated Code Example:**
```csharp
// ~/PageContentTypes/Uninews/News_Item/News_Item.generated.cs
namespace Uninews
{
    [RegisterContentTypeMapping(CONTENT_TYPE_NAME)]
    public partial class News_Item : IWebPageFieldsSource
    {
        public const string CONTENT_TYPE_NAME = "Uninews.news_Item";
        
        [SystemField]
        public WebPageFields SystemFields { get; set; }
        
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime Date_Uploaded { get; set; }
        public string Content { get; set; }
        
        // Linked Content Hub assets
        public IEnumerable<Asset> Profile { get; set; }  // ← Asset reference field
    }
}
```

**Key Points:**
- Page content types inherit from `IWebPageFieldsSource` (not `IContentItemFieldsSource`)
- Asset field type is `IEnumerable<Asset>` (linked reusable content type)
- You must have an Asset reusable content type created (see Widget section)

### 2. Page Template View - Retrieve Page Data with Assets

**CRITICAL:** Use `IContentRetriever.RetrievePages<T>` with `LinkedItemsMaxLevel = 1` to load linked assets.

**File:** `~/PageTemplates/NewsDetail/_NewsDetail.cshtml`

```cshtml
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder
@using Kentico.Web.Mvc
@using CMS.ContentEngine
@using Kentico.Content.Web.Mvc.Routing
@using System.Linq

@model TemplateViewModel<PageTemplates.NewsDetail.NewsDetailPageTemplateProperties>

@inject IWebPageDataContextRetriever webPageDataRetriever
@inject IContentRetriever contentRetriever

@{
    // Get the current page context
    var context = webPageDataRetriever.Retrieve();
    
    // Retrieve the News_Item page WITH linked items (Profile assets)
    News_Item newsItem = null;
    try
    {
        // Retrieve ALL News_Item pages with linked assets loaded
        var allPages = await contentRetriever.RetrievePages<News_Item>(
            new RetrievePagesParameters
            {
                LinkedItemsMaxLevel = 1  // ← CRITICAL: Load linked Asset items (Profile field)
            });
        
        // Find the current page by matching WebPageItemID
        newsItem = allPages.FirstOrDefault(p => 
            p.SystemFields.WebPageItemID == context.WebPage.WebPageItemID);
    }
    catch (Exception ex)
    {
        // Failed to retrieve page content - store error for debugging
        ViewData["Error"] = ex.Message;
    }
    
    ViewData["Title"] = newsItem?.Title ?? "News Article";
}

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"]</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" />
    
    <page-builder-styles />
</head>
<body>
    @if (newsItem != null)
    {
        <!-- Hero Image from Content Hub -->
        @{
            var profileAsset = newsItem.Profile?.FirstOrDefault();
            var heroImageUrl = profileAsset?.Asset1?.Url;
        }
        
        @if (!string.IsNullOrWhiteSpace(heroImageUrl))
        {
            <section class="hero-image" style="background-image: url('@heroImageUrl'); background-size: cover; background-position: center; min-height: 400px;">
                <div class="hero-overlay" style="background: rgba(26, 35, 50, 0.7); min-height: 400px; display: flex; align-items: flex-end;">
                    <div class="container py-5">
                        <h1 class="text-white fw-bold">@newsItem.Title</h1>
                        <div class="text-white-50">
                            <i class="bi bi-person-fill me-1"></i>@newsItem.Author
                            <span class="mx-2">•</span>
                            <i class="bi bi-calendar3 me-1"></i>@newsItem.Date_Uploaded.ToString("MMMM dd, yyyy")
                        </div>
                    </div>
                </div>
            </section>
        }
        
        <!-- Article Content -->
        <article class="container my-5">
            <div class="row justify-content-center">
                <div class="col-lg-8">
                    @if (!string.IsNullOrWhiteSpace(heroImageUrl) == false)
                    {
                        <!-- Show title/author here if no hero image -->
                        <h1 class="fw-bold mb-3">@newsItem.Title</h1>
                        <div class="text-muted mb-4">
                            <i class="bi bi-person-fill me-1"></i>@newsItem.Author
                            <span class="mx-2">•</span>
                            <i class="bi bi-calendar3 me-1"></i>@newsItem.Date_Uploaded.ToString("MMMM dd, yyyy")
                        </div>
                    }
                    
                    <!-- Article HTML Content -->
                    <div class="article-content">
                        @Html.Raw(newsItem.Content)
                    </div>
                </div>
            </div>
        </article>
    }
    else
    {
        <!-- Error State: Article Not Found -->
        <div class="container my-5 text-center">
            <i class="bi bi-exclamation-triangle display-1 text-danger"></i>
            <h2 class="mt-3">Article Not Found</h2>
            <p class="text-muted">The article you're looking for doesn't exist or has been removed.</p>
            <a href="/" class="btn btn-primary">Return to Home</a>
            
            @if (ViewData["Error"] != null)
            {
                <div class="alert alert-danger mt-4">
                    <strong>Error:</strong> @ViewData["Error"]
                </div>
            }
        </div>
    }
    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <page-builder-scripts />
</body>
</html>
```

### 3. Key Patterns for Retrieving Page Data with Assets

**Pattern 1: Retrieve Current Page by WebPageItemID**
```csharp
@inject IWebPageDataContextRetriever webPageDataRetriever
@inject IContentRetriever contentRetriever

@{
    // 1. Get current page context
    var context = webPageDataRetriever.Retrieve();
    
    // 2. Retrieve all pages of this type WITH linked assets
    var allPages = await contentRetriever.RetrievePages<News_Item>(
        new RetrievePagesParameters
        {
            LinkedItemsMaxLevel = 1  // Load 1 level of linked content (Profile → Asset)
        });
    
    // 3. Filter to current page
    var currentPage = allPages.FirstOrDefault(p => 
        p.SystemFields.WebPageItemID == context.WebPage.WebPageItemID);
}
```

**Pattern 2: Access Content Hub Asset URL**
```csharp
@{
    // Get first asset from Profile field (IEnumerable<Asset>)
    var profileAsset = newsItem.Profile?.FirstOrDefault();
    
    // Access URL from ContentItemAsset field
    var imageUrl = profileAsset?.Asset1?.Url;  // Direct property access
}

@if (!string.IsNullOrWhiteSpace(imageUrl))
{
    <img src="@imageUrl" alt="@newsItem.Title" class="img-fluid" />
}
```

**Pattern 3: Multiple Assets (Gallery)**
```csharp
@if (newsItem.Gallery != null && newsItem.Gallery.Any())
{
    <div class="row">
        @foreach (var asset in newsItem.Gallery)
        {
            var imageUrl = asset.Asset1?.Url;
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                <div class="col-md-4 mb-3">
                    <img src="@imageUrl" alt="Gallery image" class="img-fluid rounded" />
                </div>
            }
        }
    </div>
}
```

### 4. Comparison: Widgets vs Page Templates

| Aspect | Widgets (Content Selection) | Page Templates (Page Data) |
|--------|----------------------------|---------------------------|
| **Purpose** | Editors select assets from Content Hub | Display page's own asset fields |
| **Data Source** | `ContentItemReference` property | Current page's content type fields |
| **Retrieval Method** | `RetrieveContentByGuids<T>()` | `RetrievePages<T>()` |
| **Parameters** | `new[] { guid }` + `LinkedItemsMaxLevel = 1` | `LinkedItemsMaxLevel = 1` in `RetrievePagesParameters` |
| **Filtering** | By GUID from widget properties | By `WebPageItemID` from page context |
| **Implementation** | ViewComponent pattern | Direct injection in view |
| **Use Case** | Reusable widgets with configurable content | Article detail, profile page, case study page |

### 5. Common Patterns for Different Scenarios

**News Article Detail (Single Hero Image)**
```csharp
var heroImage = newsItem.Profile?.FirstOrDefault()?.Asset1?.Url;
```

**Product Page (Single Featured Image)**
```csharp
var featuredImage = product.FeaturedImage?.FirstOrDefault()?.Asset1?.Url;
```

**Team Member Profile (Single Photo)**
```csharp
var profilePhoto = teamMember.Photo?.FirstOrDefault()?.Asset1?.Url;
```

**Case Study (Multiple Screenshots)**
```csharp
@foreach (var screenshot in caseStudy.Screenshots ?? Enumerable.Empty<Asset>())
{
    var imageUrl = screenshot.Asset1?.Url;
    // Display image
}
```

### 6. Important Notes

**✅ DO:**
- **Always** use `LinkedItemsMaxLevel = 1` (or higher) in `RetrievePagesParameters`
- Access assets via direct property: `asset.Asset1.Url`
- Use null-conditional operators: `?.` to prevent errors
- Check for `null` or empty before rendering images
- Filter by `WebPageItemID` to get the current page

**❌ DON'T:**
- Don't forget `LinkedItemsMaxLevel = 1` - linked assets won't load without it!
- Don't use `RetrieveContentByGuids` for page data (that's for widgets)
- Don't use deprecated Media Library fields (`AssetRelatedItem`) - use `ContentItemAsset`
- Don't query by `WebPageItemGUID` - use `WebPageItemID` instead

### 7. Troubleshooting

**Problem:** Asset field is `null` or empty even though content exists

**Solution:** Ensure `LinkedItemsMaxLevel = 1` in `RetrievePagesParameters`:
```csharp
var pages = await contentRetriever.RetrievePages<News_Item>(
    new RetrievePagesParameters
    {
        LinkedItemsMaxLevel = 1  // ← REQUIRED to load linked assets!
    });
```

**Problem:** `newsItem` is `null` (page not found)

**Solution:** Verify you're filtering by correct ID:
```csharp
newsItem = allPages.FirstOrDefault(p => 
    p.SystemFields.WebPageItemID == context.WebPage.WebPageItemID);  // ← Use WebPageItemID
```

**Problem:** `Asset1` property doesn't exist

**Solution:** Regenerate code files in Kentico Admin:
1. Go to Content types → Your Asset type
2. Click **Generate code files**
3. Rebuild solution

---

## Summary: When to Use Each Approach

### Use **Widget Pattern** (Content Hub Selection) When:
- ✅ Editors need to **select** images from a library
- ✅ Same image might be used across multiple pages
- ✅ Content is configured per widget instance (e.g., Hero Banner background)
- ✅ You want reusability and flexibility

### Use **Page Template Pattern** (Page Data) When:
- ✅ Image is **owned by the page** (e.g., news article hero image, product photo)
- ✅ Image is unique to that specific page
- ✅ Image is part of the page's content structure
- ✅ Editors create content in the page form, not through Page Builder widgets

**Example Use Cases:**
- **Widget**: Hero banner with background image selected by editor
- **Page Template**: News article detail page showing the article's hero image
- **Widget**: Image gallery widget displaying selected photos
- **Page Template**: Product detail page showing product photos
- **Widget**: Team member card with selectable profile photo
- **Page Template**: Author bio page showing the author's profile photo

---

## Quick Reference: Widgets vs Page Templates with Images

| Feature | Widgets (Reusable Content) | Page Templates (Page Data) |
|---------|----------------------------|----------------------------|
| **Data Injection** | `IContentRetriever` in ViewComponent | `IContentRetriever` + `IWebPageDataContextRetriever` in view |
| **Retrieval Method** | `RetrieveContentByGuids<Asset>()` | `RetrievePages<PageType>()` |
| **Selection** | Editor selects via `ContentItemSelectorComponent` | Data comes from page content type fields |
| **Parameters** | `new[] { guid }` + `RetrieveContentParameters` | `RetrievePagesParameters` with `LinkedItemsMaxLevel = 1` |
| **Filtering** | By GUID from `ContentItemReference` | By `WebPageItemID` from page context |
| **Asset Access** | `asset.Asset1.Url` | `page.FieldName.FirstOrDefault().Asset1.Url` |
| **Use Case** | Configurable widgets (Hero Banner) | Article detail, product page, profile page |
| **Content Type** | Reusable content type (`IContentItemFieldsSource`) | Page content type (`IWebPageFieldsSource`) |

namespace Components.Widgets.HeroBannerWidget
{
    // ViewComponent class defined in HeroBannerWidgetViewComponent.cs
}
```

**Key Difference from Standard Widget:**
- Use `viewComponentType` parameter (NOT `customViewName`)
- ViewComponent handles data retrieval and view model creation
- Allows async data fetching (Content Hub retrieval)

## Content Hub Asset Retrieval Patterns

### Pattern 1: Single Asset by GUID
```csharp
var assets = await contentRetriever.RetrieveContentByGuids<Uninews.Asset>(
    new[] { imageGuid },
    new RetrieveContentParameters { LinkedItemsMaxLevel = 1 });

var assetUrl = assets.FirstOrDefault()?.Asset1?.Url;
```

### Pattern 2: Multiple Assets
```csharp
var imageGuids = properties.Images.Select(i => i.Identifier).ToArray();

var assets = await contentRetriever.RetrieveContentByGuids<Uninews.Asset>(
    imageGuids,
    new RetrieveContentParameters { LinkedItemsMaxLevel = 1 });

foreach (var asset in assets)
{
    var url = asset.Asset1?.Url;
    var fileName = asset.Asset1?.Metadata?.FileName;
    var fileSize = asset.Asset1?.Metadata?.Size;
    // ...
}
```

### Pattern 3: Asset Metadata
```csharp
if (asset?.Asset1 != null)
{
    var url = asset.Asset1.Url;
    var fileName = asset.Asset1.Metadata?.FileName;
    var extension = asset.Asset1.Metadata?.Extension;
    var size = asset.Asset1.Metadata?.Size;
    var width = asset.Asset1.Metadata?.Width;   // For images
    var height = asset.Asset1.Metadata?.Height; // For images
}
```

## Common Mistakes to Avoid

### ❌ Using Deprecated Media Library
**Wrong:**
```csharp
// OLD - Media Library (deprecated)
public IEnumerable<AssetRelatedItem> Asset1 { get; set; }
```

**Right:**
```csharp
// NEW - Content Hub Asset
public ContentItemAsset Asset1 { get; set; }
```

### ❌ Wrong Retrieval Method
**Wrong:**
```csharp
// Wrong - for multiple content types
var assets = await contentRetriever.RetrieveContentOfContentTypesByGuids<IContentItemFieldsSource>(
    new[] { "Uninews.asset" },
    new[] { imageGuid },
    new RetrieveContentOfContentTypesParameters { ... });
```

**Right:**
```csharp
// Correct - for single known content type
var assets = await contentRetriever.RetrieveContentByGuids<Uninews.Asset>(
    new[] { imageGuid },
    new RetrieveContentParameters { LinkedItemsMaxLevel = 1 });
```

### ❌ Missing Null Checks
**Wrong:**
```csharp
var url = assets.First().Asset1.Url;  // Can throw NullReferenceException
```

**Right:**
```csharp
var url = assets.FirstOrDefault()?.Asset1?.Url;  // Safe navigation
if (!string.IsNullOrWhiteSpace(url))
{
    // Use the URL
}
```

### ❌ Not Handling Retrieval Failures
**Wrong:**
```csharp
var assets = await contentRetriever.RetrieveContentByGuids<Uninews.Asset>(...);
var url = assets.FirstOrDefault()?.Asset1?.Url;
```

**Right:**
```csharp
try
{
    var assets = await contentRetriever.RetrieveContentByGuids<Uninews.Asset>(...);
    backgroundImageUrl = assets.FirstOrDefault()?.Asset1?.Url;
}
catch (Exception)
{
    // Fallback to default/null
    backgroundImageUrl = null;
}
```

## Testing Checklist

- [ ] Asset content type created with `ContentItemAsset` field (NOT Media Library)
- [ ] Code files regenerated after creating content type
- [ ] Asset selector appears in widget configuration dialog
- [ ] Can select/browse assets from Content Hub
- [ ] Selected asset displays correctly on live site
- [ ] Fallback works when no asset is selected
- [ ] Image URLs are properly formatted (absolute/relative)
- [ ] No errors in Event Log when selecting/displaying assets
- [ ] ViewComponent properly injects `IContentRetriever`
- [ ] Null checks prevent exceptions

## Reference

- [Kentico Content Hub Documentation](https://docs.kentico.com/documentation/business-users/content-hub)
- [ContentItemSelectorComponent Reference](https://docs.kentico.com/developers-and-admins/customization/extend-the-administration-interface/ui-form-components/reference-admin-ui-form-components#combined-content-selector)
- [Content Retrieval API](https://docs.kentico.com/documentation/developers-and-admins/development/content-retrieval/retrieve-content-items)

**Right:**
```cshtml
@if (!string.IsNullOrWhiteSpace(Model.Properties.Title))
{
    <h2>@Model.Properties.Title</h2>
}
```

### ❌ Including Page Builder Scripts in Widget
**Wrong:**
```cshtml
@model ComponentViewModel<MyWidgetProperties>

<div>Widget content</div>

<page-builder-scripts />  @* Don't include in widgets! *@
```

**Right:**
```cshtml
@model ComponentViewModel<MyWidgetProperties>

<div>Widget content</div>
@* Scripts are in the page template, not the widget *@
```

### ❌ Missing Underscore Prefix on View File
**Wrong:** `HeroBannerWidget.cshtml`
**Right:** `_HeroBannerWidget.cshtml`

### ❌ Not Setting Order Property
**Wrong:**
```csharp
[TextInputComponent(Label = "Title")]  // Fields appear in random order
public string Title { get; set; }
```

**Right:**
```csharp
[TextInputComponent(Order = 0, Label = "Title")]
public string Title { get; set; }
```

## Widget Reference Documentation

- [Widgets for Page Builder](https://docs.kentico.com/documentation/developers-and-admins/development/builders/page-builder/widgets-for-page-builder)
- [Widget Properties](https://docs.kentico.com/documentation/developers-and-admins/development/builders/page-builder/widgets-for-page-builder/widget-properties)
- [Example Widget Development](https://docs.kentico.com/documentation/developers-and-admins/development/builders/page-builder/widgets-for-page-builder/example-widget-development)
- [Reference - Admin UI Form Components](https://docs.kentico.com/documentation/developers-and-admins/customization/extend-the-administration-interface/ui-form-components/reference-admin-ui-form-components)

