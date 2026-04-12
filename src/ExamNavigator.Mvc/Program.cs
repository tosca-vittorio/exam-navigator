using ExamNavigator.Application.Services;
using ExamNavigator.Infrastructure.PostgreSql;

namespace ExamNavigator.Mvc;

public static class Program
{
    private const string PostgreSqlHost = "localhost";
    private const int PostgreSqlPort = 5432;
    private const string PostgreSqlDatabase = "exam_navigator";
    private const string PostgreSqlUsername = "exam_navigator_app";
    private const string PostgreSqlPasswordEnvironmentVariable = "EXAM_NAVIGATOR_PG_PASSWORD";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IExamNavigationService>(
            _ => new PostgreSqlExamNavigationService(BuildPostgreSqlConnectionString()));

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

    private static string BuildPostgreSqlConnectionString()
    {
        var password = Environment.GetEnvironmentVariable(PostgreSqlPasswordEnvironmentVariable);
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Set the EXAM_NAVIGATOR_PG_PASSWORD environment variable before starting the MVC host.");
        }

        return "Host=" + PostgreSqlHost
            + ";Port=" + PostgreSqlPort
            + ";Database=" + PostgreSqlDatabase
            + ";Username=" + PostgreSqlUsername
            + ";Password=" + password;
    }
}
