using ExamNavigator.Application.Contracts;

namespace ExamNavigator.Mvc.Models;

public sealed class ExamNavigationCommandInputModel
{
    public int? SelectedRoomId { get; set; }

    public int? SelectedBodyPartId { get; set; }

    public int? SelectedExamId { get; set; }

    public string SearchText { get; set; } = string.Empty;

    public ExamSearchField SearchField { get; set; } = ExamSearchField.DescrizioneEsame;

    public string SelectionState { get; set; } = string.Empty;

    public int? SelectedGridIndex { get; set; }
}

public sealed class SelectedExamRowViewModel
{
    public string CodiceMinisteriale { get; set; } = string.Empty;

    public string CodiceInterno { get; set; } = string.Empty;

    public string DescrizioneEsame { get; set; } = string.Empty;

    public string BodyPartLabel { get; set; } = string.Empty;

    public string RoomLabel { get; set; } = string.Empty;
}

public sealed class ExamNavigationPageViewModel
{
    public IReadOnlyList<LookupItem> Rooms { get; set; } = Array.Empty<LookupItem>();

    public IReadOnlyList<LookupItem> BodyParts { get; set; } = Array.Empty<LookupItem>();

    public IReadOnlyList<ExamListItem> Exams { get; set; } = Array.Empty<ExamListItem>();

    public int? SelectedRoomId { get; set; }

    public int? SelectedBodyPartId { get; set; }

    public int? SelectedExamId { get; set; }

    public string SearchText { get; set; } = string.Empty;

    public ExamSearchField SearchField { get; set; } = ExamSearchField.DescrizioneEsame;

    public IReadOnlyList<SelectedExamRowViewModel> SelectedExams { get; set; } = Array.Empty<SelectedExamRowViewModel>();

    public string SelectionState { get; set; } = string.Empty;

    public int? SelectedGridIndex { get; set; }
}

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
