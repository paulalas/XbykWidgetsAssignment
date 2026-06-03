# Page Template Creation & Page Builder Integration Guide
## Xperience by Kentico

This document explains the architecture and step-by-step process for creating page templates, connecting them to layouts, and integrating with the Page Builder system.

---

## Architecture Overview

### Three-Layer Structure

```
┌─────────────────────────────────────────┐
│   _Layout.cshtml (Site Chrome)          │
│   - Header/Navigation                   │
│   - Footer                              │
│   - Global Styles/Scripts               │
│   - @RenderBody() ← Injects Template    │
└─────────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────────┐
│   _TemplateName.cshtml (Page Structure) │
│   - Page-specific layout                │
│   - <editable-area /> zones             │
│   - Template-specific styles            │
└─────────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────────┐
│   Page Builder Widgets                  │
│   - Added via Admin UI                  │
│   - Rendered in editable areas          │
│   - Reusable components                 │
└─────────────────────────────────────────┘
```

### How They Connect

1. **_ViewStart.cshtml** (in PageTemplates folder) specifies the layout:
   ```csharp
   @{
       Layout = "~/Views/Shared/_Layout.cshtml";
   }
   ```

2. **_Layout.cshtml** calls `@RenderBody()` which injects the page template

3. **Page Template** defines structure with `<editable-area />` zones

4. **Page Builder** allows content editors to add widgets to editable areas

---

## Step-by-Step: Creating a Page Template

### Step 1: Create Template Registration Class

**Location:** `~/PageTemplates/[TemplateName]/[TemplateName]PageTemplate.cs`

**Purpose:** Registers the template with Kentico's Page Builder system

**Example:** `PageTemplates/Home/HomePageTemplate.cs`

```csharp
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

[assembly: RegisterPageTemplate(
    identifier: "Uninews.Home_Default",          // Unique identifier (project.template_name)
    name: "Home - Default",                      // Display name in admin UI
    customViewName: "~/PageTemplates/Home/_Home.cshtml",  // Path to view (must use ~/)
    Description = "Default template for Home page",       // Tooltip description
    IconClass = "xp-layout")]                            // Icon in admin UI

namespace PageTemplates.Home
{
    // Empty namespace - properties classes would go here if needed
}
```

**Key Parameters:**
- `identifier`: Format as `ProjectName.PageType_TemplateName` (must be unique)
- `customViewName`: MUST start with `~/` and point to underscore-prefixed view
- `IconClass`: Kentico icon classes (xp-layout, xp-doc, xp-newspaper, etc.)

### Step 2: Create Template View

**Location:** `~/PageTemplates/[TemplateName]/_[TemplateName].cshtml`

**CRITICAL:** Filename MUST start with underscore `_`

**Example:** `PageTemplates/Home/_Home.cshtml`

```cshtml
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder
@using Kentico.Web.Mvc

@model TemplateViewModel

@{
    ViewData["Title"] = "Home";
}

<!-- Template-specific styles (optional) -->
<style>
    /* Scoped styles for this template */
    .section-title {
        font-size: 1.75rem;
        font-weight: 700;
    }
</style>

<!-- Editable Area: Hero Section -->
<editable-area area-identifier="hero-banner" />

<!-- Editable Area: Main Content -->
<section class="py-5">
    <div class="container">
        <editable-area area-identifier="main-content" />
    </div>
</section>

<!-- Editable Area: Secondary Content -->
<section class="py-5 bg-light">
    <div class="container">
        <editable-area area-identifier="secondary-content" />
    </div>
</section>
```

**Required Elements:**
1. ✅ `@model TemplateViewModel` (or `TemplateViewModel<PropertiesType>` if using properties)
2. ✅ `ViewData["Title"]` for page title
3. ✅ `<editable-area area-identifier="unique-name" />` for Page Builder zones
4. ✅ Underscore prefix on filename

**DO NOT Include:**
- ❌ Header/Navigation (in _Layout.cshtml)
- ❌ Footer (in _Layout.cshtml)
- ❌ `<page-builder-styles />` (in _Layout.cshtml)
- ❌ `<page-builder-scripts />` (in _Layout.cshtml)

### Step 3: Connect to Layout via _ViewStart.cshtml

**Location:** `~/PageTemplates/_ViewStart.cshtml`

**Purpose:** Automatically applies _Layout.cshtml to all page templates

```csharp
@{
    Layout = "~/Views/Shared/_Layout.cshtml";
}
```

**How It Works:**
- ASP.NET Core executes `_ViewStart.cshtml` before rendering any view in the same folder
- Sets the `Layout` property for all templates in `PageTemplates/` folder
- Template content is injected where `@RenderBody()` appears in the layout

---

## Understanding the Layout File

### _Layout.cshtml Structure

**Location:** `~/Views/Shared/_Layout.cshtml`

**Anatomy:**

```cshtml
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>@ViewData["Title"] - UniNews</title>
    
    <!-- ✅ REQUIRED: Page Builder CSS -->
    <page-builder-styles />
    
    <!-- Your global CSS -->
    <link href="..." rel="stylesheet" />
</head>
<body>
    <!-- Header/Navigation (Global) -->
    <nav class="navbar">
        <!-- Navigation markup -->
    </nav>

    <!-- ✅ CRITICAL: This is where page templates render -->
    <main class="main-content">
        @RenderBody()
    </main>

    <!-- Footer (Global) -->
    <footer class="main-footer">
        <!-- Footer markup -->
    </footer>

    <!-- Your global JS -->
    <script src="..."></script>
    
    <!-- ✅ REQUIRED: Page Builder JavaScript -->
    <page-builder-scripts />
    
    <!-- Optional scripts from templates/views -->
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

**Critical Elements:**
1. `<page-builder-styles />` in `<head>` - Loads widget CSS
2. `@RenderBody()` in `<main>` - Injects page template content
3. `<page-builder-scripts />` before `</body>` - Loads widget JavaScript
4. `@RenderSectionAsync("Scripts", required: false)` - Optional page-specific scripts

**Rendering Flow:**
```
Request → _ViewStart.cshtml sets Layout 
       → _Layout.cshtml loads
       → @RenderBody() executes
       → Page Template renders with editable areas
       → Widgets render inside editable areas
```

---

## Page Builder Integration

### Understanding Editable Areas

**Purpose:** Define zones where content editors can add/arrange widgets via the admin UI

**Syntax:**
```cshtml
<editable-area area-identifier="unique-identifier" />
```

**Best Practices:**

1. **Use Descriptive Identifiers**
   ```cshtml
   <!-- ✅ Good -->
   <editable-area area-identifier="hero-banner" />
   <editable-area area-identifier="main-content" />
   <editable-area area-identifier="sidebar" />
   
   <!-- ❌ Avoid -->
   <editable-area area-identifier="area1" />
   <editable-area area-identifier="zone" />
   ```

2. **Wrap in Semantic HTML**
   ```cshtml
   <section class="py-5 bg-light">
       <div class="container">
           <editable-area area-identifier="featured-content" />
       </div>
   </section>
   ```

3. **Plan Layout Structure**
   - Hero/Banner areas (full-width)
   - Main content areas (container-wrapped)
   - Sidebar areas (grid columns)
   - Footer callout areas

### Common Tag Helper Errors

**✅ CORRECT:**
```cshtml
<editable-area area-identifier="main" />
```

**❌ WRONG (Old syntax):**
```cshtml
<page-builder-editable-area area-identifier="main" />
```

---

## Complete Example: News Listing Template

### 1. Registration Class
**File:** `PageTemplates/NewsListing/NewsListingPageTemplate.cs`

```csharp
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

[assembly: RegisterPageTemplate(
    identifier: "Uninews.NewsListing_Default",
    name: "News Listing - Default",
    customViewName: "~/PageTemplates/NewsListing/_NewsListing.cshtml",
    Description = "Template for news listing pages with breadcrumbs and listing area",
    IconClass = "xp-newspaper")]

namespace PageTemplates.NewsListing
{
}
```

### 2. Template View
**File:** `PageTemplates/NewsListing/_NewsListing.cshtml`

```cshtml
@using Kentico.PageBuilder.Web.Mvc
@using Kentico.Content.Web.Mvc
@using Kentico.Content.Web.Mvc.PageBuilder

@model TemplateViewModel

@{
    ViewData["Title"] = "News";
}

<!-- Breadcrumb Area -->
<div class="breadcrumb-bar">
    <div class="container">
        <editable-area area-identifier="breadcrumb" />
    </div>
</div>

<!-- Title Banner -->
<editable-area area-identifier="title-banner" />

<!-- News Listing -->
<section class="py-5">
    <div class="container">
        <editable-area area-identifier="news-listing" />
    </div>
</section>
```

### 3. How Content Editors Use It

**In Kentico Admin UI:**

1. Create/Edit a News page
2. Select "News Listing - Default" template
3. Click "Edit Page" to enter Page Builder mode
4. See three editable areas:
   - **breadcrumb** - Add BreadcrumbWidget
   - **title-banner** - Add TitleBannerWidget
   - **news-listing** - Add NewsListingWidget
5. Configure each widget's properties
6. Preview and publish

---

## Template Properties (Advanced)

### When to Use Properties

Use template properties when you need page-level settings that affect the entire template (not just a widget).

**Example:** Background color, layout variant, column count

### Creating a Template with Properties

**1. Define Properties Class**

```csharp
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

[assembly: RegisterPageTemplate(
    identifier: "Uninews.CustomLayout",
    name: "Custom Layout",
    propertiesType: typeof(PageTemplates.Custom.CustomLayoutProperties),
    customViewName: "~/PageTemplates/Custom/_CustomLayout.cshtml",
    Description = "Layout with configurable columns",
    IconClass = "xp-layout-3-col")]

namespace PageTemplates.Custom
{
    public class CustomLayoutProperties : IPageTemplateProperties
    {
        [EditingComponent(DropDownComponent.IDENTIFIER, Label = "Number of Columns")]
        public string ColumnCount { get; set; } = "2";
    }
}
```

**2. Access Properties in View**

```cshtml
@model TemplateViewModel<PageTemplates.Custom.CustomLayoutProperties>

@{
    var columns = Model.Properties.ColumnCount;
    var columnClass = columns == "3" ? "col-md-4" : "col-md-6";
}

<div class="row">
    <div class="@columnClass">
        <editable-area area-identifier="column-1" />
    </div>
    <div class="@columnClass">
        <editable-area area-identifier="column-2" />
    </div>
    @if (columns == "3")
    {
        <div class="@columnClass">
            <editable-area area-identifier="column-3" />
        </div>
    }
</div>
```

---

## Namespace Convention (CRITICAL)

### ✅ CORRECT Pattern

```csharp
namespace PageTemplates.Home        // Template-specific namespace
namespace PageTemplates.NewsListing // Template-specific namespace
namespace PageTemplates.About       // Template-specific namespace
```

### ❌ WRONG Pattern

```csharp
namespace BiolerPage               // Content type namespace - CONFLICTS!
namespace Uninews.Home             // Can cause assembly conflicts
```

**Why This Matters:**
- Content types have their own namespaces
- Templates need separate namespaces to avoid collisions
- Use format: `PageTemplates.[TemplateName]`

---

## Troubleshooting

### Template Not Appearing in Admin UI

**Check:**
1. ✅ Registered with `[assembly: RegisterPageTemplate(...)]`
2. ✅ `customViewName` path is correct and starts with `~/`
3. ✅ View file has underscore prefix (`_TemplateName.cshtml`)
4. ✅ Project has been rebuilt
5. ✅ Content type is associated with template (if scoped)

### Layout Not Rendering

**Check:**
1. ✅ `_ViewStart.cshtml` exists in `PageTemplates/` folder
2. ✅ Layout path is correct: `~/Views/Shared/_Layout.cshtml`
3. ✅ `@RenderBody()` exists in layout file

### Widgets Not Rendering

**Check:**
1. ✅ `<page-builder-styles />` in layout's `<head>`
2. ✅ `<page-builder-scripts />` in layout before `</body>`
3. ✅ Using `<editable-area />` not `<page-builder-editable-area />`
4. ✅ Widgets are registered and enabled

### Duplicate Headers/Footers

**Problem:** Header appears twice

**Solution:** Remove header/footer from page template - only keep in `_Layout.cshtml`

---

## Quick Reference Checklist

### Creating a New Page Template

- [ ] Create folder: `PageTemplates/[TemplateName]/`
- [ ] Create registration: `[TemplateName]PageTemplate.cs`
  - [ ] Use unique identifier: `Project.PageType_Template`
  - [ ] Set customViewName with `~/` prefix
  - [ ] Use `PageTemplates.[TemplateName]` namespace
- [ ] Create view: `_[TemplateName].cshtml` (underscore required!)
  - [ ] Add `@model TemplateViewModel`
  - [ ] Set `ViewData["Title"]`
  - [ ] Add `<editable-area area-identifier="..." />` zones
  - [ ] NO header/footer/global scripts
- [ ] Verify `_ViewStart.cshtml` exists in `PageTemplates/`
- [ ] Rebuild project
- [ ] Test in Kentico admin UI

### Layout File Must Have

- [ ] `<page-builder-styles />` in `<head>`
- [ ] `@RenderBody()` in `<main>` or similar
- [ ] `<page-builder-scripts />` before `</body>`
- [ ] `@await RenderSectionAsync("Scripts", required: false)` (optional)

---

## File Structure Example

```
~/PageTemplates/
├── _ViewStart.cshtml                    ← Sets Layout for all templates
├── _ViewImports.cshtml                  ← Common using statements
├── Home/
│   ├── HomePageTemplate.cs              ← Registration
│   └── _Home.cshtml                     ← View (underscore!)
├── About/
│   ├── AboutPageTemplate.cs
│   └── _About.cshtml
├── NewsListing/
│   ├── NewsListingPageTemplate.cs
│   └── _NewsListing.cshtml
└── NewsDetail/
    ├── NewsDetailPageTemplate.cs
    └── _NewsDetail.cshtml

~/Views/Shared/
└── _Layout.cshtml                       ← Global layout with @RenderBody()
```

---

## Summary

1. **Page Templates** define page structure with editable areas
2. **_Layout.cshtml** provides global chrome (header/footer)
3. **_ViewStart.cshtml** connects templates to layout
4. **@RenderBody()** is where template content renders
5. **<editable-area />** zones enable Page Builder functionality
6. **Widgets** are added by content editors in admin UI
7. **Separation of Concerns:** Layout = chrome, Template = structure, Widgets = content

This architecture provides:
- ✅ Reusable global layouts
- ✅ Flexible page structures
- ✅ Content editor empowerment via Page Builder
- ✅ Clean separation of concerns
- ✅ Maintainable codebase
