namespace FormBuilder.Application;

public static class FieldTypes
{
    public static readonly string[] Supported =
    [
        "text", "textarea", "number", "date", "datetime",
        "email", "phone", "select", "radio", "checkbox", "file"
    ];

    public static readonly string[] TypesWithOptions = ["select", "radio"];

    public const string Pattern = "^(text|textarea|number|date|datetime|email|phone|select|radio|checkbox|file)$";

    public const string ErrorMessage =
        "Type must be one of: text, textarea, number, date, datetime, email, phone, select, radio, checkbox, file";

    public static bool RequiresOptions(string type) => TypesWithOptions.Contains(type);
}
