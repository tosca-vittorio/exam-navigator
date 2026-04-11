using System;
using System.Collections.Generic;
using System.IO;

namespace ExamNavigator.WinForms
{
    internal sealed class IniConfigurationDocument
    {
        public Dictionary<string, Dictionary<string, string>> Sections { get; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        public static IniConfigurationDocument Load(string filePath)
        {
            var document = new IniConfigurationDocument();

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                return document;
            }

            string currentSectionName = null;

            foreach (var rawLine in File.ReadAllLines(filePath))
            {
                var line = (rawLine ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                if (line.StartsWith("[", StringComparison.Ordinal)
                    && line.EndsWith("]", StringComparison.Ordinal)
                    && line.Length > 2)
                {
                    currentSectionName = line.Substring(1, line.Length - 2).Trim();

                    if (string.IsNullOrWhiteSpace(currentSectionName))
                    {
                        currentSectionName = null;
                        continue;
                    }

                    if (!document.Sections.ContainsKey(currentSectionName))
                    {
                        document.Sections[currentSectionName] =
                            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    }

                    continue;
                }

                if (string.IsNullOrWhiteSpace(currentSectionName))
                {
                    continue;
                }

                var separatorIndex = line.IndexOf('=');
                if (separatorIndex <= 0)
                {
                    continue;
                }

                var key = line.Substring(0, separatorIndex).Trim();
                var value = line.Substring(separatorIndex + 1).Trim();

                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                document.Sections[currentSectionName][key] = value;
            }

            return document;
        }
    }
}
