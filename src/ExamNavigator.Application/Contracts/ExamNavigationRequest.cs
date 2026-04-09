namespace ExamNavigator.Application.Contracts
{
    public sealed class ExamNavigationRequest
    {
        public int? SelectedRoomId { get; set; }

        public int? SelectedBodyPartId { get; set; }

        public string SearchText { get; set; } = string.Empty;

        public ExamSearchField SearchField { get; set; } = ExamSearchField.DescrizioneEsame;
    }
}
