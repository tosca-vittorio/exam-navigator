using System;
using System.Collections.Generic;
using ExamNavigator.Application.Contracts;
using ExamNavigator.Application.Services;
using Npgsql;

namespace ExamNavigator.Infrastructure.PostgreSql
{
    public sealed class PostgreSqlExamNavigationService : IExamNavigationService
    {
        private const string SelectVisibleRoomsSql = @"
SELECT DISTINCT
    r.id,
    r.name
FROM public.room r
INNER JOIN public.exam_room er ON er.room_id = r.id
INNER JOIN public.exam e ON e.id = er.exam_id
WHERE
    @search_text IS NULL
    OR (
        @search_field = 'CodiceMinisteriale'
        AND e.codice_ministeriale ILIKE '%' || @search_text || '%'
    )
    OR (
        @search_field = 'CodiceInterno'
        AND e.codice_interno ILIKE '%' || @search_text || '%'
    )
    OR (
        @search_field = 'DescrizioneEsame'
        AND e.descrizione_esame ILIKE '%' || @search_text || '%'
    )
ORDER BY r.name;";

        private const string SelectVisibleBodyPartsSql = @"
SELECT DISTINCT
    bp.id,
    bp.name
FROM public.body_part bp
INNER JOIN public.exam e ON e.body_part_id = bp.id
INNER JOIN public.exam_room er ON er.exam_id = e.id
WHERE
    (
        @search_text IS NULL
        OR (
            @search_field = 'CodiceMinisteriale'
            AND e.codice_ministeriale ILIKE '%' || @search_text || '%'
        )
        OR (
            @search_field = 'CodiceInterno'
            AND e.codice_interno ILIKE '%' || @search_text || '%'
        )
        OR (
            @search_field = 'DescrizioneEsame'
            AND e.descrizione_esame ILIKE '%' || @search_text || '%'
        )
    )
    AND (@selected_room_id IS NULL OR er.room_id = @selected_room_id)
ORDER BY bp.name;";

        private const string SelectVisibleExamsSql = @"
SELECT DISTINCT
    e.id,
    e.codice_ministeriale,
    e.codice_interno,
    e.descrizione_esame,
    e.body_part_id
FROM public.exam e
INNER JOIN public.exam_room er ON er.exam_id = e.id
WHERE
    (
        @search_text IS NULL
        OR (
            @search_field = 'CodiceMinisteriale'
            AND e.codice_ministeriale ILIKE '%' || @search_text || '%'
        )
        OR (
            @search_field = 'CodiceInterno'
            AND e.codice_interno ILIKE '%' || @search_text || '%'
        )
        OR (
            @search_field = 'DescrizioneEsame'
            AND e.descrizione_esame ILIKE '%' || @search_text || '%'
        )
    )
    AND (@selected_room_id IS NULL OR er.room_id = @selected_room_id)
    AND (@selected_body_part_id IS NULL OR e.body_part_id = @selected_body_part_id)
ORDER BY e.descrizione_esame, e.codice_interno;";

        private readonly string _connectionString;

        public PostgreSqlExamNavigationService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("The PostgreSQL connection string cannot be null, empty, or whitespace.", nameof(connectionString));
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

            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = new NpgsqlCommand(SelectVisibleRoomsSql, connection))
            {
                command.Parameters.AddWithValue("search_text", (object)normalizedSearchText ?? DBNull.Value);
                command.Parameters.AddWithValue("search_field", searchField.ToString());

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

            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = new NpgsqlCommand(SelectVisibleBodyPartsSql, connection))
            {
                command.Parameters.AddWithValue("search_text", (object)normalizedSearchText ?? DBNull.Value);
                command.Parameters.AddWithValue("search_field", searchField.ToString());
                command.Parameters.AddWithValue("selected_room_id", (object)selectedRoomId ?? DBNull.Value);

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

            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = new NpgsqlCommand(SelectVisibleExamsSql, connection))
            {
                command.Parameters.AddWithValue("search_text", (object)normalizedSearchText ?? DBNull.Value);
                command.Parameters.AddWithValue("search_field", searchField.ToString());
                command.Parameters.AddWithValue("selected_room_id", (object)selectedRoomId ?? DBNull.Value);
                command.Parameters.AddWithValue("selected_body_part_id", (object)selectedBodyPartId ?? DBNull.Value);

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
