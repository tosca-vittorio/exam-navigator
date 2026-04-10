using ExamNavigator.Application.Contracts;

namespace ExamNavigator.WinForms
{
    internal static class Predefiniti_Ricerca
    {
        public static string SearchText { get; set; } = string.Empty;

        public static ExamSearchField SearchField { get; set; } = ExamSearchField.DescrizioneEsame;
    }
}
