using System.Collections.Generic;

namespace ExamNavigator.Application.Contracts
{
    public sealed class ExamNavigationResult
    {
        public IReadOnlyList<LookupItem> Rooms { get; set; } = new List<LookupItem>();

        public IReadOnlyList<LookupItem> BodyParts { get; set; } = new List<LookupItem>();

        public IReadOnlyList<ExamListItem> Exams { get; set; } = new List<ExamListItem>();

        public int? SelectedRoomId { get; set; }

        public int? SelectedBodyPartId { get; set; }
    }
}
