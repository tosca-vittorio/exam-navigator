using ExamNavigator.Application.Contracts;

namespace ExamNavigator.Application.Services
{
    public interface IExamNavigationService
    {
        ExamNavigationResult GetNavigation(ExamNavigationRequest request);
    }
}
