using System.Diagnostics;
using ExamNavigator.Application.Contracts;
using ExamNavigator.Application.Services;
using ExamNavigator.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamNavigator.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IExamNavigationService _navigationService;

    public HomeController(IExamNavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
    }

    [HttpGet]
    public IActionResult Index(
        int? selectedRoomId,
        int? selectedBodyPartId,
        string? searchText,
        ExamSearchField searchField = ExamSearchField.DescrizioneEsame)
    {
        var request = new ExamNavigationRequest
        {
            SelectedRoomId = selectedRoomId,
            SelectedBodyPartId = selectedBodyPartId,
            SearchText = searchText ?? string.Empty,
            SearchField = searchField
        };

        var result = _navigationService.GetNavigation(request);

        var viewModel = new ExamNavigationPageViewModel
        {
            Rooms = result.Rooms,
            BodyParts = result.BodyParts,
            Exams = result.Exams,
            SelectedRoomId = result.SelectedRoomId,
            SelectedBodyPartId = result.SelectedBodyPartId,
            SearchText = request.SearchText,
            SearchField = request.SearchField
        };

        return View(viewModel);
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
}
