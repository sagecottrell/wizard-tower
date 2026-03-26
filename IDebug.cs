using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace wizardtower;

internal interface IDebug
{
    int Indent => 4;

    string DebugString(bool shouldIndent = true, int indent = 0, int depth = -1)
    {
        var p = GetType().GetProperties();
        var i2 = new string(' ', indent + Indent);
        var s = p.Select(p => {
            var val = p.GetValue(this);
            if (val is IDebug d && depth > 0)
                val = d.DebugString(shouldIndent, indent + Indent, depth < 0 ? -1 : depth--);
            else
                val = DebuggerDisplayHelper.GetDebuggerDisplayString(val);
            if (shouldIndent)
                return $"\n{i2}{p.Name}={val}";
            return $"{p.Name}={val}";
        }).ToArray();
        return $"{GetType().Name}{{ {string.Join(", ", s)}{(shouldIndent ? "\n" : " ")}}}";
    }
}

public static partial class DebuggerDisplayHelper
{
    public static string? GetDebuggerDisplayString(object? obj)
    {
        if (obj is null)
            return "null";

        Type type = obj.GetType();
        var displayAttribute = type.GetCustomAttribute<DebuggerDisplayAttribute>();

        if (displayAttribute == null)
        {
            // If no DebuggerDisplay, return default ToString() or type name
            return obj.ToString();
        }

        string formatString = displayAttribute.Value;

        // Use regex to find expressions inside curly braces, e.g., "{Name}" or "{Count, h}"
        // The regex captures the expression inside the braces
        var matches = regex().Matches(formatString);

        string result = formatString;

        foreach (Match match in matches)
        {
            // The captured expression (e.g., "Name" or "Count, h")
            string expression = match.Groups[1].Value.Trim();

            // Handle optional format specifiers (like ",h" for hex)
            string propertyName = expression.Split(',')[0].Trim();
            var formatSpecifier = expression.Contains(',') ? expression.Split(',')[1].Trim() : null;

            // Evaluate the property/field value
            var value = GetValueFromMember(obj, propertyName);

            // Format the value if a specifier is present, otherwise convert to string
            var formattedValue = value?.ToString();
            if (formatSpecifier != null)
            {
                // Note: Complex format specifiers (like ",h") are handled by the debugger's evaluation engine, 
                // and replicating all of them perfectly via simple reflection can be complex.
                // A better approach for complex formatting is to use a private property/method.
            }

            // Replace the original brace expression with the value
            result = result.Replace(match.Value, formattedValue);
        }

        return result;
    }

    private static object? GetValueFromMember(object obj, string memberName)
    {
        // Try to get property
        var prop = obj.GetType().GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (prop != null)
            return prop.GetValue(obj);

        // Try to get field
        var field = obj.GetType().GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
            return field.GetValue(obj);

        // Try to invoke method (parameterless)
        var method = obj.GetType().GetMethod(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
        if (method != null)
            return method.Invoke(obj, null);

        return null;
    }

    [GeneratedRegex(@"\{([^}]+)\}")]
    private static partial Regex regex();
}