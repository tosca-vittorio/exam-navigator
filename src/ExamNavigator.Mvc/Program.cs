using ExamNavigator.Application.Services;
using ExamNavigator.Infrastructure.SqlServer;

namespace ExamNavigator.Mvc;

public static class Program
{
    private const string SqlServerConnectionStringEnvironmentVariable = "EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IExamNavigationService>(
            _ => new SqlServerExamNavigationService(BuildSqlServerConnectionString()));

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

    private static string BuildSqlServerConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable(SqlServerConnectionStringEnvironmentVariable);
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Set the EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING environment variable before starting the MVC host.");
        }

        return connectionString;
    }
}
