using System;
using System.Collections.Generic;
using System.Data;
using ExamNavigator.Application.Contracts;
using ExamNavigator.Application.Services;
using Microsoft.Data.SqlClient;

namespace ExamNavigator.Infrastructure.SqlServer
{
    public sealed class SqlServerExamNavigationService : IExamNavigationService
    {
        private const string SelectVisibleRoomsSql = @"
SELECT DISTINCT
    r.Id,
    r.Name
FROM dbo.Room r
INNER JOIN dbo.ExamRoom er ON er.RoomId = r.Id
INNER JOIN dbo.Exam e ON e.Id = er.ExamId
WHERE
    @search_text IS NULL
    OR (
        @search_field = N'CodiceMinisteriale'
        AND UPPER(e.CodiceMinisteriale) LIKE N'%' + UPPER(@search_text) + N'%'
    )
    OR (
        @search_field = N'CodiceInterno'
        AND UPPER(e.CodiceInterno) LIKE N'%' + UPPER(@search_text) + N'%'
    )
    OR (
        @search_field = N'DescrizioneEsame'
        AND UPPER(e.DescrizioneEsame) LIKE N'%' + UPPER(@search_text) + N'%'
    )
ORDER BY r.Name;";

        private const string SelectVisibleBodyPartsSql = @"
SELECT DISTINCT
    bp.Id,
    bp.Name
FROM dbo.BodyPart bp
INNER JOIN dbo.Exam e ON e.BodyPartId = bp.Id
INNER JOIN dbo.ExamRoom er ON er.ExamId = e.Id
WHERE
    (
        @search_text IS NULL
        OR (
            @search_field = N'CodiceMinisteriale'
            AND UPPER(e.CodiceMinisteriale) LIKE N'%' + UPPER(@search_text) + N'%'
        )
        OR (
            @search_field = N'CodiceInterno'
            AND UPPER(e.CodiceInterno) LIKE N'%' + UPPER(@search_text) + N'%'
        )
        OR (
            @search_field = N'DescrizioneEsame'
            AND UPPER(e.DescrizioneEsame) LIKE N'%' + UPPER(@search_text) + N'%'
        )
    )
    AND (@selected_room_id IS NULL OR er.RoomId = @selected_room_id)
ORDER BY bp.Name;";

        private const string SelectVisibleExamsSql = @"
SELECT DISTINCT
    e.Id,
    e.CodiceMinisteriale,
    e.CodiceInterno,
    e.DescrizioneEsame,
    e.BodyPartId
FROM dbo.Exam e
INNER JOIN dbo.ExamRoom er ON er.ExamId = e.Id
WHERE
    (
        @search_text IS NULL
        OR (
            @search_field = N'CodiceMinisteriale'
            AND UPPER(e.CodiceMinisteriale) LIKE N'%' + UPPER(@search_text) + N'%'
        )
        OR (
            @search_field = N'CodiceInterno'
            AND UPPER(e.CodiceInterno) LIKE N'%' + UPPER(@search_text) + N'%'
        )
        OR (
            @search_field = N'DescrizioneEsame'
            AND UPPER(e.DescrizioneEsame) LIKE N'%' + UPPER(@search_text) + N'%'
        )
    )
    AND (@selected_room_id IS NULL OR er.RoomId = @selected_room_id)
    AND (@selected_body_part_id IS NULL OR e.BodyPartId = @selected_body_part_id)
ORDER BY e.DescrizioneEsame, e.CodiceInterno;";

        private readonly string _connectionString;

        public SqlServerExamNavigationService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("The SQL Server connection string cannot be null, empty, or whitespace.", nameof(connectionString));
            }

            _connectionString = connectionString;
        }

        public ExamNavigationResult GetNavigation(ExamNavigationRequest request)
        {
            request = request ?? new ExamNavigationRequest();

            var normalizedSearchText = NormalizeSearchText(request.SearchText);
            var visibleRooms = LoadVisibleRooms(normalizedSearchText, request.SearchField);
            var selectedRoomId = ResolveSelectedRoomId(visibleRooms, request.SelectedRoomId);
            var visibleBodyParts = LoadVisibleBodyParts(normalizedSearchText, request.SearchField, selectedRoomId);
            var selectedBodyPartId = ResolveSelectedBodyPartId(visibleBodyParts, request.SelectedBodyPartId);
            var visibleExams = LoadVisibleExams(
                normalizedSearchText,
                request.SearchField,
                selectedRoomId,
                selectedBodyPartId);

            return new ExamNavigationResult
            {
                Rooms = visibleRooms,
                BodyParts = visibleBodyParts,
                Exams = visibleExams,
                SelectedRoomId = selectedRoomId,
                SelectedBodyPartId = selectedBodyPartId
            };
        }

        private List<LookupItem> LoadVisibleRooms(string normalizedSearchText, ExamSearchField searchField)
        {
            var rooms = new List<LookupItem>();

            using (var connection = CreateConnection())
            using (var command = new SqlCommand(SelectVisibleRoomsSql, connection))
            {
                AddSearchParameters(command, normalizedSearchText, searchField);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rooms.Add(new LookupItem
                        {
                            Id = reader.GetInt32(0),
                            Label = reader.GetString(1)
                        });
                    }
                }
            }

            return rooms;
        }

        private List<LookupItem> LoadVisibleBodyParts(
            string normalizedSearchText,
            ExamSearchField searchField,
            int? selectedRoomId)
        {
            var bodyParts = new List<LookupItem>();

            using (var connection = CreateConnection())
            using (var command = new SqlCommand(SelectVisibleBodyPartsSql, connection))
            {
                AddSearchParameters(command, normalizedSearchText, searchField);
                AddNullableIntParameter(command, "@selected_room_id", selectedRoomId);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bodyParts.Add(new LookupItem
                        {
                            Id = reader.GetInt32(0),
                            Label = reader.GetString(1)
                        });
                    }
                }
            }

            return bodyParts;
        }

        private List<ExamListItem> LoadVisibleExams(
            string normalizedSearchText,
            ExamSearchField searchField,
            int? selectedRoomId,
            int? selectedBodyPartId)
        {
            var exams = new List<ExamListItem>();

            using (var connection = CreateConnection())
            using (var command = new SqlCommand(SelectVisibleExamsSql, connection))
            {
                AddSearchParameters(command, normalizedSearchText, searchField);
                AddNullableIntParameter(command, "@selected_room_id", selectedRoomId);
                AddNullableIntParameter(command, "@selected_body_part_id", selectedBodyPartId);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        exams.Add(new ExamListItem
                        {
                            Id = reader.GetInt32(0),
                            CodiceMinisteriale = reader.GetString(1),
                            CodiceInterno = reader.GetString(2),
                            DescrizioneEsame = reader.GetString(3),
                            BodyPartId = reader.GetInt32(4)
                        });
                    }
                }
            }

            return exams;
        }

        private static void AddSearchParameters(
            SqlCommand command,
            string normalizedSearchText,
            ExamSearchField searchField)
        {
            var searchTextParameter = command.Parameters.Add("@search_text", SqlDbType.NVarChar, 100);
            searchTextParameter.Value = (object)normalizedSearchText ?? DBNull.Value;

            var searchFieldParameter = command.Parameters.Add("@search_field", SqlDbType.NVarChar, 30);
            searchFieldParameter.Value = searchField.ToString();
        }

        private static void AddNullableIntParameter(SqlCommand command, string parameterName, int? value)
        {
            var parameter = command.Parameters.Add(parameterName, SqlDbType.Int);
            parameter.Value = (object)value ?? DBNull.Value;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private static int? ResolveSelectedRoomId(IReadOnlyList<LookupItem> visibleRooms, int? requestedRoomId)
        {
            if (requestedRoomId.HasValue)
            {
                for (var index = 0; index < visibleRooms.Count; index++)
                {
                    if (visibleRooms[index].Id == requestedRoomId.Value)
                    {
                        return requestedRoomId;
                    }
                }
            }

            return visibleRooms.Count > 0 ? (int?)visibleRooms[0].Id : null;
        }

        private static int? ResolveSelectedBodyPartId(
            IReadOnlyList<LookupItem> visibleBodyParts,
            int? requestedBodyPartId)
        {
            if (requestedBodyPartId.HasValue)
            {
                for (var index = 0; index < visibleBodyParts.Count; index++)
                {
                    if (visibleBodyParts[index].Id == requestedBodyPartId.Value)
                    {
                        return requestedBodyPartId;
                    }
                }
            }

            return visibleBodyParts.Count > 0 ? (int?)visibleBodyParts[0].Id : null;
        }

        private static string NormalizeSearchText(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return null;
            }

            return searchText.Trim();
        }
    }
}
