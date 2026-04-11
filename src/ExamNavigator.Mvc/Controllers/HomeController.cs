using System.Diagnostics;
using System.Text;
using ExamNavigator.Application.Contracts;
using ExamNavigator.Application.Services;
using ExamNavigator.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace ExamNavigator.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IExamNavigationService _navigationService;

    public HomeController(IExamNavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
    }

    [HttpGet]
    public IActionResult Index([FromQuery] ExamNavigationCommandInputModel input)
    {
        return View(BuildPageModel(input));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ApplySelectionCommand(ExamNavigationCommandInputModel input, string command)
    {
        var selectedExams = DeserializeSelectionState(input.SelectionState);
        var selectedGridIndex = NormalizeSelectedGridIndex(input.SelectedGridIndex, selectedExams.Count);

        switch ((command ?? string.Empty).Trim().ToLowerInvariant())
        {
            case "confirm":
            {
                var currentPage = BuildPageModel(input, selectedExams, selectedGridIndex);
                var currentExam = currentPage.Exams.FirstOrDefault(exam => exam.Id == currentPage.SelectedExamId);
                var currentBodyPart = currentPage.BodyParts.FirstOrDefault(bodyPart => bodyPart.Id == currentPage.SelectedBodyPartId);
                var currentRoom = currentPage.Rooms.FirstOrDefault(room => room.Id == currentPage.SelectedRoomId);

                if (currentExam is not null && currentBodyPart is not null && currentRoom is not null)
                {
                    selectedExams.Add(new SelectedExamRowViewModel
                    {
                        CodiceMinisteriale = currentExam.CodiceMinisteriale,
                        CodiceInterno = currentExam.CodiceInterno,
                        DescrizioneEsame = currentExam.DescrizioneEsame,
                        BodyPartLabel = currentBodyPart.Label,
                        RoomLabel = currentRoom.Label
                    });

                    selectedGridIndex = selectedExams.Count - 1;
                }

                break;
            }

            case "remove":
            {
                if (selectedGridIndex.HasValue)
                {
                    selectedExams.RemoveAt(selectedGridIndex.Value);

                    if (selectedExams.Count == 0)
                    {
                        selectedGridIndex = null;
                    }
                    else if (selectedGridIndex.Value >= selectedExams.Count)
                    {
                        selectedGridIndex = selectedExams.Count - 1;
                    }
                }

                break;
            }

            case "up":
            {
                if (selectedGridIndex.HasValue && selectedGridIndex.Value > 0)
                {
                    var index = selectedGridIndex.Value;
                    (selectedExams[index - 1], selectedExams[index]) = (selectedExams[index], selectedExams[index - 1]);
                    selectedGridIndex = index - 1;
                }

                break;
            }

            case "down":
            {
                if (selectedGridIndex.HasValue && selectedGridIndex.Value < selectedExams.Count - 1)
                {
                    var index = selectedGridIndex.Value;
                    (selectedExams[index + 1], selectedExams[index]) = (selectedExams[index], selectedExams[index + 1]);
                    selectedGridIndex = index + 1;
                }

                break;
            }
        }

        return View("Index", BuildPageModel(input, selectedExams, selectedGridIndex));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }

    private ExamNavigationPageViewModel BuildPageModel(
        ExamNavigationCommandInputModel? input,
        IReadOnlyList<SelectedExamRowViewModel>? selectedExamsOverride = null,
        int? selectedGridIndexOverride = null)
    {
        input ??= new ExamNavigationCommandInputModel();

        var request = new ExamNavigationRequest
        {
            SelectedRoomId = input.SelectedRoomId,
            SelectedBodyPartId = input.SelectedBodyPartId,
            SearchText = input.SearchText ?? string.Empty,
            SearchField = input.SearchField
        };

        var result = _navigationService.GetNavigation(request);

        var selectedExamId = input.SelectedExamId;
        if (!selectedExamId.HasValue || result.Exams.All(exam => exam.Id != selectedExamId.Value))
        {
            selectedExamId = result.Exams.Count > 0 ? result.Exams[0].Id : null;
        }

        var selectedExams = selectedExamsOverride is null
            ? DeserializeSelectionState(input.SelectionState)
            : new List<SelectedExamRowViewModel>(selectedExamsOverride);

        var selectedGridIndex = NormalizeSelectedGridIndex(
            selectedGridIndexOverride ?? input.SelectedGridIndex,
            selectedExams.Count);

        return new ExamNavigationPageViewModel
        {
            Rooms = result.Rooms,
            BodyParts = result.BodyParts,
            Exams = result.Exams,
            SelectedRoomId = result.SelectedRoomId,
            SelectedBodyPartId = result.SelectedBodyPartId,
            SelectedExamId = selectedExamId,
            SearchText = input.SearchText ?? string.Empty,
            SearchField = input.SearchField,
            SelectedExams = selectedExams,
            SelectionState = SerializeSelectionState(selectedExams),
            SelectedGridIndex = selectedGridIndex
        };
    }

    private static int? NormalizeSelectedGridIndex(int? selectedGridIndex, int itemCount)
    {
        if (!selectedGridIndex.HasValue || itemCount <= 0)
        {
            return null;
        }

        return selectedGridIndex.Value >= 0 && selectedGridIndex.Value < itemCount
            ? selectedGridIndex.Value
            : null;
    }

    private static string SerializeSelectionState(IReadOnlyList<SelectedExamRowViewModel> selectedExams)
    {
        if (selectedExams is null || selectedExams.Count == 0)
        {
            return string.Empty;
        }

        return string.Join(
            ";",
            selectedExams.Select(exam => string.Join(
                "|",
                EncodeToken(exam.CodiceMinisteriale),
                EncodeToken(exam.CodiceInterno),
                EncodeToken(exam.DescrizioneEsame),
                EncodeToken(exam.BodyPartLabel),
                EncodeToken(exam.RoomLabel))));
    }

    private static List<SelectedExamRowViewModel> DeserializeSelectionState(string? selectionState)
    {
        var selectedExams = new List<SelectedExamRowViewModel>();
        if (string.IsNullOrWhiteSpace(selectionState))
        {
            return selectedExams;
        }

        foreach (var rowToken in selectionState.Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            var cells = rowToken.Split('|');
            if (cells.Length != 5)
            {
                continue;
            }

            selectedExams.Add(new SelectedExamRowViewModel
            {
                CodiceMinisteriale = DecodeToken(cells[0]),
                CodiceInterno = DecodeToken(cells[1]),
                DescrizioneEsame = DecodeToken(cells[2]),
                BodyPartLabel = DecodeToken(cells[3]),
                RoomLabel = DecodeToken(cells[4])
            });
        }

        return selectedExams;
    }

    private static string EncodeToken(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
        return WebEncoders.Base64UrlEncode(bytes);
    }

    private static string DecodeToken(string value)
    {
        try
        {
            var bytes = WebEncoders.Base64UrlDecode(value);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return string.Empty;
        }
    }
}
