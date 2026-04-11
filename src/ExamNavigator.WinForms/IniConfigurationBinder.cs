using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ExamNavigator.WinForms
{
    internal static class IniConfigurationBinder
    {
        public static void Apply(IniConfigurationDocument document)
        {
            if (document == null)
            {
                return;
            }

            var assembly = typeof(IniConfigurationBinder).Assembly;

            foreach (var sectionEntry in document.Sections)
            {
                if (string.IsNullOrWhiteSpace(sectionEntry.Key))
                {
                    continue;
                }

                var targetType = ResolveSectionType(assembly, sectionEntry.Key);
                if (targetType == null)
                {
                    continue;
                }

                foreach (var settingEntry in sectionEntry.Value)
                {
                    ApplySetting(targetType, settingEntry.Key, settingEntry.Value);
                }
            }
        }

        private static Type ResolveSectionType(Assembly assembly, string sectionName)
        {
            var expectedTypeName = "Predefiniti_" + sectionName.Trim();

            return assembly
                .GetTypes()
                .FirstOrDefault(type =>
                    type.IsClass
                    && type.IsAbstract
                    && type.IsSealed
                    && string.Equals(type.Namespace, typeof(IniConfigurationBinder).Namespace, StringComparison.Ordinal)
                    && string.Equals(type.Name, expectedTypeName, StringComparison.OrdinalIgnoreCase));
        }

        private static void ApplySetting(Type targetType, string propertyName, string rawValue)
        {
            if (targetType == null || string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            var property = targetType.GetProperty(
                propertyName.Trim(),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);

            if (property == null || !property.CanWrite || property.GetIndexParameters().Length > 0)
            {
                return;
            }

            object convertedValue;
            if (!TryConvertValue(rawValue, property.PropertyType, out convertedValue))
            {
                return;
            }

            property.SetValue(null, convertedValue, null);
        }

        private static bool TryConvertValue(string rawValue, Type targetType, out object convertedValue)
        {
            convertedValue = null;

            if (targetType == null)
            {
                return false;
            }

            if (targetType == typeof(string))
            {
                string stringValue;
                if (!TryParseQuotedString(rawValue, out stringValue))
                {
                    return false;
                }

                convertedValue = stringValue;
                return true;
            }

            if (targetType == typeof(int))
            {
                int intValue;
                if (!int.TryParse(
                    (rawValue ?? string.Empty).Trim(),
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out intValue))
                {
                    return false;
                }

                convertedValue = intValue;
                return true;
            }

            if (targetType == typeof(bool))
            {
                bool boolValue;
                if (!TryParseBoolean(rawValue, out boolValue))
                {
                    return false;
                }

                convertedValue = boolValue;
                return true;
            }

            if (targetType.IsEnum)
            {
                object enumValue;
                if (!TryParseEnum(rawValue, targetType, out enumValue))
                {
                    return false;
                }

                convertedValue = enumValue;
                return true;
            }

            return false;
        }

        private static bool TryParseQuotedString(string rawValue, out string value)
        {
            value = null;

            var trimmedValue = (rawValue ?? string.Empty).Trim();
            if (trimmedValue.Length < 2
                || trimmedValue[0] != '"'
                || trimmedValue[trimmedValue.Length - 1] != '"')
            {
                return false;
            }

            value = trimmedValue.Substring(1, trimmedValue.Length - 2);
            return true;
        }

        private static bool TryParseBoolean(string rawValue, out bool value)
        {
            var trimmedValue = (rawValue ?? string.Empty).Trim();

            if (string.Equals(trimmedValue, "1", StringComparison.Ordinal))
            {
                value = true;
                return true;
            }

            if (string.Equals(trimmedValue, "0", StringComparison.Ordinal))
            {
                value = false;
                return true;
            }

            return bool.TryParse(trimmedValue, out value);
        }

        private static bool TryParseEnum(string rawValue, Type enumType, out object enumValue)
        {
            enumValue = null;

            string quotedEnumName;
            if (TryParseQuotedString(rawValue, out quotedEnumName))
            {
                return TryCreateEnumFromName(enumType, quotedEnumName, out enumValue);
            }

            int numericValue;
            if (int.TryParse(
                (rawValue ?? string.Empty).Trim(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out numericValue))
            {
                if (!Enum.IsDefined(enumType, numericValue))
                {
                    return false;
                }

                enumValue = Enum.ToObject(enumType, numericValue);
                return true;
            }

            return TryCreateEnumFromName(enumType, (rawValue ?? string.Empty).Trim(), out enumValue);
        }

        private static bool TryCreateEnumFromName(Type enumType, string enumName, out object enumValue)
        {
            enumValue = null;

            if (string.IsNullOrWhiteSpace(enumName))
            {
                return false;
            }

            try
            {
                var parsedValue = Enum.Parse(enumType, enumName.Trim(), true);

                if (!Enum.IsDefined(enumType, parsedValue))
                {
                    return false;
                }

                enumValue = parsedValue;
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
