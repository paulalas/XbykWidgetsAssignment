# Get Started Form Setup Instructions

This document explains how to set up the "Get Started" form in Kentico Xperience to collect form submissions from the website.

## Overview

The Get Started form collects information from potential students interested in courses. The form data is stored in Kentico's form system using the BizForm API.

## Step 1: Create the Form in Kentico Admin

1. **Navigate to Forms**
   - Log into the Kentico admin interface
   - Go to **Content** → **Forms** (or **Marketing** → **Forms** depending on your version)

2. **Create New Form**
   - Click **New form**
   - Set the following properties:
     - **Form name**: Get Started
     - **Form code name**: `GetStarted` (important: this must match exactly!)
     - **Form display name**: Get Started Form
     - **Description**: Collects information from students interested in our courses

3. **Add Form Fields**
   Add the following fields with these exact field names:

   | Field Name | Data Type | Size | Required | Settings |
   |------------|-----------|------|----------|----------|
   | `FullName` | Text | 200 | Yes | Label: "Full Name" |
   | `Email` | Text | 200 | Yes | Label: "Email", Validation: Email address |
   | `SelectedCourse` | Text | 100 | Yes | Label: "Selected Course" |
   | `UserMessage` | Long text | 1000 | No | Label: "Message" |
   | `AgreedToTerms` | Boolean | - | Yes | Label: "Agreed to Terms" |
   | `SubmittedOn` | Date and time | - | Yes | Label: "Submitted On", Default: Current time |

4. **Configure Form Settings**
   - **Notifications**: Set up email notifications if desired
   - **After submission**: You can redirect users or show a message
   - **Double opt-in**: Enable if required for GDPR compliance

5. **Save the Form**
   - Click **Save** to create the form

## Step 2: Verify Code Integration

The following files have been created to handle form submissions:

### 1. Form Model (`Models/GetStartedFormModel.cs`)
Validates the incoming form data from the client.

### 2. Forms Controller (`Controllers/FormsController.cs`)
Handles the API endpoint `/api/forms/get-started` that receives form submissions.

### 3. JavaScript (`wwwroot/js/site.js`)
Handles form submission via AJAX with validation and user feedback.

### 4. Updated Modal (`Views/Shared/_Layout.cshtml`)
The modal form with proper form ID and alert placeholder for messages.

## Step 3: Test the Form

1. **Build and run the application**
   ```bash
   dotnet build
   dotnet run
   ```

2. **Open the website**
   - Navigate to your website's homepage
   - Click the "Get Started" button in the navigation

3. **Fill out the form**
   - Enter test data in all required fields
   - Check "I agree to the terms and conditions"
   - Click **Submit**

4. **Verify submission**
   - You should see a success message
   - The modal should close after 2 seconds
   - In Kentico admin, go to **Content** → **Forms** → **Get Started**
   - Click **View data** to see your submission

## Field Mapping

The form fields in Kentico map to the model properties as follows:

| Model Property | Kentico Field | Notes |
|----------------|---------------|-------|
| `Name` | `FullName` | User's full name |
| `Email` | `Email` | User's email address |
| `Course` | `SelectedCourse` | Selected course option |
| `Message` | `UserMessage` | Optional message from user |
| `AgreeToTerms` | `AgreedToTerms` | Boolean for terms acceptance |
| N/A | `SubmittedOn` | Auto-populated with current timestamp |

## Customization

### Adding New Fields

1. Add the field in Kentico admin (Content → Forms → Get Started → Fields)
2. Add the property to `GetStartedFormModel.cs`
3. Update the controller to set the value: `newFormItem.SetValue("FieldName", model.PropertyName);`
4. Add the input to the modal form in `_Layout.cshtml`
5. Update the JavaScript to include the field in `formData`

### Email Notifications

To send email notifications when forms are submitted:

1. In Kentico admin, go to **Content** → **Forms** → **Get Started**
2. Go to the **Notifications** tab
3. Click **New email notification**
4. Configure recipient, subject, and email template
5. Use macro expressions to include form data: `{%FullName%}`, `{%Email%}`, etc.

### Export Data

To export form submissions:

1. Go to **Content** → **Forms** → **Get Started**
2. Click **Export data**
3. Choose format (CSV, Excel, XML)
4. Select fields to export
5. Download the file

## Troubleshooting

### "Form configuration not found" error
- Verify the form code name is exactly `GetStarted` in Kentico admin
- Check that the form is published and not in draft mode

### Form data not saving
- Ensure all required fields are filled
- Check field names match exactly between code and Kentico
- Review application logs for detailed error messages

### Validation errors
- Verify field types match (text, boolean, datetime)
- Check field size limits in Kentico match model validation
- Ensure email field has proper validation rule

## Security Considerations

- The form includes server-side validation in the model and controller
- CSRF protection is automatically handled by ASP.NET Core
- Consider adding CAPTCHA for production to prevent spam
- Implement rate limiting to prevent abuse
- Sanitize user input before displaying anywhere

## Next Steps

- Set up email notifications in Kentico
- Configure CAPTCHA for spam protection
- Add analytics tracking for form submissions
- Create automated workflows based on form data
- Set up follow-up email sequences
