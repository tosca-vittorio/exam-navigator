using ExamNavigator.Application.Contracts;

namespace ExamNavigator.Mvc.Models;

public sealed class ExamNavigationPageViewModel
{
    public IReadOnlyList<LookupItem> Rooms { get; init; } = Array.Empty<LookupItem>();

    public IReadOnlyList<LookupItem> BodyParts { get; init; } = Array.Empty<LookupItem>();

    public IReadOnlyList<ExamListItem> Exams { get; init; } = Array.Empty<ExamListItem>();

    public int? SelectedRoomId { get; init; }

    public int? SelectedBodyPartId { get; init; }

    public string SearchText { get; init; } = string.Empty;

    public ExamSearchField SearchField { get; init; } = ExamSearchField.DescrizioneEsame;
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
