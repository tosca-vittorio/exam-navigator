using ExamNavigator.Application.Contracts;
using ExamNavigator.Application.Services;

namespace ExamNavigator.Mvc;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IExamNavigationService, BootstrapNavigationService>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}

internal sealed class BootstrapNavigationService : IExamNavigationService
{
    private readonly List<LookupItem> _rooms = new()
    {
        new LookupItem { Id = 1, Label = "Ecografia Massimino" },
        new LookupItem { Id = 2, Label = "Ecografia Privitera" },
        new LookupItem { Id = 3, Label = "Radiologia" },
        new LookupItem { Id = 4, Label = "Risonanza" }
    };

    private readonly List<LookupItem> _bodyParts = new()
    {
        new LookupItem { Id = 1, Label = "Addome" },
        new LookupItem { Id = 2, Label = "Arti superiori" },
        new LookupItem { Id = 3, Label = "Testa" }
    };

    private readonly List<BootstrapExamRecord> _exams = new()
    {
        new BootstrapExamRecord
        {
            Id = 1,
            CodiceMinisteriale = "ECOADD01",
            CodiceInterno = "INT001",
            DescrizioneEsame = "Eco Addome",
            BodyPartId = 1,
            RoomIds = new List<int> { 1, 2 }
        },
        new BootstrapExamRecord
        {
            Id = 2,
            CodiceMinisteriale = "RXMANODX",
            CodiceInterno = "INT002",
            DescrizioneEsame = "RX mano Dx",
            BodyPartId = 2,
            RoomIds = new List<int> { 3 }
        },
        new BootstrapExamRecord
        {
            Id = 3,
            CodiceMinisteriale = "RMNCRAN",
            CodiceInterno = "INT003",
            DescrizioneEsame = "RMN cranio",
            BodyPartId = 3,
            RoomIds = new List<int> { 4 }
        },
        new BootstrapExamRecord
        {
            Id = 4,
            CodiceMinisteriale = "ECOTIROI",
            CodiceInterno = "INT004",
            DescrizioneEsame = "Eco tiroide",
            BodyPartId = 3,
            RoomIds = new List<int> { 1, 2 }
        }
    };

    public ExamNavigationResult GetNavigation(ExamNavigationRequest request)
    {
        request ??= new ExamNavigationRequest();

        var normalizedSearchText = (request.SearchText ?? string.Empty).Trim();

        var filteredExams = _exams
            .Where(exam => MatchesSearch(exam, normalizedSearchText, request.SearchField))
            .ToList();

        var visibleRooms = _rooms
            .Where(room => filteredExams.Any(exam => exam.RoomIds.Contains(room.Id)))
            .OrderBy(room => room.Label)
            .ToList();

        var selectedRoomId = request.SelectedRoomId;
        if (!selectedRoomId.HasValue || visibleRooms.All(room => room.Id != selectedRoomId.Value))
        {
            selectedRoomId = visibleRooms.Count > 0 ? visibleRooms[0].Id : null;
        }

        var visibleBodyParts = _bodyParts
            .Where(bodyPart => filteredExams.Any(
                exam => exam.BodyPartId == bodyPart.Id
                    && (!selectedRoomId.HasValue || exam.RoomIds.Contains(selectedRoomId.Value))))
            .OrderBy(bodyPart => bodyPart.Label)
            .ToList();

        var selectedBodyPartId = request.SelectedBodyPartId;
        if (!selectedBodyPartId.HasValue || visibleBodyParts.All(bodyPart => bodyPart.Id != selectedBodyPartId.Value))
        {
            selectedBodyPartId = visibleBodyParts.Count > 0 ? visibleBodyParts[0].Id : null;
        }

        var visibleExams = filteredExams
            .Where(exam =>
                (!selectedRoomId.HasValue || exam.RoomIds.Contains(selectedRoomId.Value))
                && (!selectedBodyPartId.HasValue || exam.BodyPartId == selectedBodyPartId.Value))
            .OrderBy(exam => exam.DescrizioneEsame)
            .ThenBy(exam => exam.CodiceInterno)
            .Select(MapExamListItem)
            .ToList();

        return new ExamNavigationResult
        {
            Rooms = visibleRooms,
            BodyParts = visibleBodyParts,
            Exams = visibleExams,
            SelectedRoomId = selectedRoomId,
            SelectedBodyPartId = selectedBodyPartId
        };
    }

    private static bool MatchesSearch(
        BootstrapExamRecord exam,
        string normalizedSearchText,
        ExamSearchField searchField)
    {
        if (string.IsNullOrWhiteSpace(normalizedSearchText))
        {
            return true;
        }

        var comparison = StringComparison.OrdinalIgnoreCase;

        return searchField switch
        {
            ExamSearchField.CodiceMinisteriale =>
                exam.CodiceMinisteriale.IndexOf(normalizedSearchText, comparison) >= 0,

            ExamSearchField.CodiceInterno =>
                exam.CodiceInterno.IndexOf(normalizedSearchText, comparison) >= 0,

            _ =>
                exam.DescrizioneEsame.IndexOf(normalizedSearchText, comparison) >= 0
        };
    }

    private static ExamListItem MapExamListItem(BootstrapExamRecord exam)
    {
        return new ExamListItem
        {
            Id = exam.Id,
            CodiceMinisteriale = exam.CodiceMinisteriale,
            CodiceInterno = exam.CodiceInterno,
            DescrizioneEsame = exam.DescrizioneEsame,
            BodyPartId = exam.BodyPartId
        };
    }

    private sealed class BootstrapExamRecord
    {
        public int Id { get; set; }

        public string CodiceMinisteriale { get; set; } = string.Empty;

        public string CodiceInterno { get; set; } = string.Empty;

        public string DescrizioneEsame { get; set; } = string.Empty;

        public int BodyPartId { get; set; }

        public List<int> RoomIds { get; set; } = new();
    }
}
