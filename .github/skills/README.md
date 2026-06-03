# Kentico Widget Creation Skills

This folder contains AI skills from the [KentiCopilot project](https://github.com/Kentico/xperience-by-kentico-kenticopilot) to help you create Page Builder widgets for Xperience by Kentico.

## What's Included

### 1. **widget-create-research**
Prepares for widget implementation by analyzing requirements, validating against Kentico documentation, and generating detailed instructions.

**Location:** `.github/skills/widget-create-research/`

**Usage:**
1. Create a folder with your widget requirements (text files, mockups, etc.)
2. Invoke the skill: `@workspace /widget-create-research path/to/requirements-folder`
3. The AI will generate a detailed implementation plan

### 2. **widget-create-implementation**
Creates the actual widget code following generated instructions and project conventions.

**Location:** `.github/skills/widget-create-implementation/`

**Usage:**
1. After the research phase generates instructions
2. Invoke the skill: `@workspace /widget-create-implementation path/to/instructions.md`
3. The AI will create all widget files (ViewComponent, Properties, ViewModel, View)

## Two-Stage Workflow

```
Step 1: Research Phase
┌─────────────────────────────┐
│ Your Requirements           │
│ - Widget description        │
│ - Design mockups           │
│ - Feature list             │
└─────────────┬───────────────┘
              │
              ▼
┌─────────────────────────────┐
│ @widget-create-research     │
│ - Validates requirements    │
│ - Checks Kentico docs       │
│ - Analyzes project patterns │
└─────────────┬───────────────┘
              │
              ▼
┌─────────────────────────────┐
│ Instructions.md             │
│ - Complete widget spec      │
│ - Implementation checklist  │
└─────────────────────────────┘

Step 2: Implementation Phase
┌─────────────────────────────┐
│ Instructions.md             │
└─────────────┬───────────────┘
              │
              ▼
┌─────────────────────────────┐
│ @widget-create-implementation│
│ - Creates all files         │
│ - Follows project conventions│
│ - Implements best practices │
└─────────────┬───────────────┘
              │
              ▼
┌─────────────────────────────┐
│ Widget Code                 │
│ - ViewComponent.cs          │
│ - Properties.cs             │
│ - ViewModel.cs              │
│ - _View.cshtml              │
└─────────────────────────────┘
```

## Prerequisites

- **Kentico MCP Server** must be configured in `.vscode/mcp.json`
- Current configuration:
  ```json
  {
    "servers": {
      "kentico": {
        "url": "https://docs.kentico.com/mcp",
        "type": "http"
      }
    }
  }
  ```

## Reference Files

The skills include:
- **Documentation Links** - Official Kentico docs for widgets, content retrieval, form components
- **Example Widgets** - Card Widget and Call-to-Action Widget implementations
- **Base Instructions** - Important rules and best practices
- **Creation Template** - Structured template for widget requirements

## How AI Discovers These Skills

GitHub Copilot automatically discovers skills in:
- `.github/skills/` folder (recommended)
- `.github/copilot/` folder
- Root-level `SKILL.md` files

Each skill has a YAML frontmatter with:
- `name` - Unique skill identifier
- `description` - What the skill does
- `argument-hint` - What input it expects

## Example Usage

### Creating a Hero Banner Widget

1. **Create requirements folder:**
   ```
   widget-requirements/
   ├── description.txt
   └── mockup.png
   ```

2. **Ask AI to research:**
   > "Use the widget-create-research skill on the widget-requirements folder"

3. **Review generated instructions** in the requirements folder

4. **Ask AI to implement:**
   > "Use the widget-create-implementation skill on the instructions file"

## Benefits

- ✅ **Consistent patterns** - Follows your project's existing widget structure
- ✅ **Best practices** - Implements Kentico recommendations automatically
- ✅ **Documentation-aware** - Validates against official Kentico docs via MCP
- ✅ **Complete implementation** - Creates all necessary files and registrations
- ✅ **Localization-ready** - Includes resource strings and proper null handling

## Source

These skills are from the official Kentico KentiCopilot project:
https://github.com/Kentico/xperience-by-kentico-kenticopilot

## Learn More

- [KentiCopilot Documentation](https://github.com/Kentico/xperience-by-kentico-kenticopilot)
- [Kentico Page Builder Widgets](https://docs.kentico.com/documentation/developers-and-admins/development/builders/page-builder/widgets-for-page-builder)
- [Widget Development Example](https://docs.kentico.com/documentation/developers-and-admins/development/builders/page-builder/widgets-for-page-builder/example-widget-development)
