using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExamNavigator.Application.Contracts;
using ExamNavigator.Application.Services;
using ExamNavigator.Infrastructure.SqlServer;

namespace ExamNavigator.WinForms
{
    internal sealed class BootstrapNavigationService : IExamNavigationService
    {
        private readonly List<LookupItem> _rooms = new List<LookupItem>
        {
            new LookupItem { Id = 1, Label = "EcografiaMassimino" },
            new LookupItem { Id = 2, Label = "EcografiaPrivitera" },
            new LookupItem { Id = 3, Label = "Radiologia" },
            new LookupItem { Id = 4, Label = "Risonanza" }
        };

        private readonly List<LookupItem> _bodyParts = new List<LookupItem>
        {
            new LookupItem { Id = 1, Label = "Addome" },
            new LookupItem { Id = 2, Label = "Arti superiori" },
            new LookupItem { Id = 3, Label = "Testa" }
        };

        private readonly List<BootstrapExamRecord> _exams = new List<BootstrapExamRecord>
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
            request = request ?? new ExamNavigationRequest();

            var normalizedSearchText = (request.SearchText ?? string.Empty).Trim();
            var hasSearch = !string.IsNullOrWhiteSpace(normalizedSearchText);

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
                selectedRoomId = visibleRooms.Count > 0 ? (int?)visibleRooms[0].Id : null;
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
                selectedBodyPartId = visibleBodyParts.Count > 0 ? (int?)visibleBodyParts[0].Id : null;
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

            switch (searchField)
            {
                case ExamSearchField.CodiceMinisteriale:
                    return exam.CodiceMinisteriale.IndexOf(normalizedSearchText, comparison) >= 0;

                case ExamSearchField.CodiceInterno:
                    return exam.CodiceInterno.IndexOf(normalizedSearchText, comparison) >= 0;

                case ExamSearchField.DescrizioneEsame:
                default:
                    return exam.DescrizioneEsame.IndexOf(normalizedSearchText, comparison) >= 0;
            }
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

            public List<int> RoomIds { get; set; } = new List<int>();
        }
    }

    internal static class Program
    {
        private const string DefaultIniSearchPattern = "*.ini";
        private const string SqlServerConnectionStringEnvironmentVariable = "EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING";

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LoadConfigurationDefaults();

            var navigationService = new SqlServerExamNavigationService(BuildSqlServerConnectionString());

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Form1(navigationService));
        }

        private static string BuildSqlServerConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable(SqlServerConnectionStringEnvironmentVariable);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Set the EXAM_NAVIGATOR_SQLSERVER_CONNECTION_STRING environment variable before starting the WinForms host.");
            }

            return connectionString;
        }

        private static void LoadConfigurationDefaults()
        {
            var configurationFilePath = ResolveConfigurationFilePath();
            if (string.IsNullOrWhiteSpace(configurationFilePath))
            {
                return;
            }

            var document = IniConfigurationDocument.Load(configurationFilePath);
            IniConfigurationBinder.Apply(document);
        }

        private static string ResolveConfigurationFilePath()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (string.IsNullOrWhiteSpace(baseDirectory) || !Directory.Exists(baseDirectory))
            {
                return null;
            }

            var configurationFiles = Directory.GetFiles(baseDirectory, DefaultIniSearchPattern);
            if (configurationFiles.Length == 0)
            {
                return null;
            }

            Array.Sort(configurationFiles, StringComparer.OrdinalIgnoreCase);
            return configurationFiles[0];
        }
    }
}
