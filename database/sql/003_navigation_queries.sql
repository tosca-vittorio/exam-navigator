/*
    Query di riferimento per la navigazione a cascata richiesta dal test.

    Parametri previsti dal chiamante:
    @SearchText NVARCHAR(100) = NULL
    @SearchField NVARCHAR(30) = NULL  -- CodiceMinisteriale | CodiceInterno | DescrizioneEsame
    @SelectedRoomId INT = NULL
    @SelectedBodyPartId INT = NULL
*/

DECLARE @SearchText NVARCHAR(100) = NULL;
DECLARE @SearchField NVARCHAR(30) = NULL;
DECLARE @SelectedRoomId INT = NULL;
DECLARE @SelectedBodyPartId INT = NULL;

DECLARE @NormalizedSearchText NVARCHAR(100) =
    CASE
        WHEN @SearchText IS NULL OR LTRIM(RTRIM(@SearchText)) = N'' THEN NULL
        ELSE LTRIM(RTRIM(@SearchText))
    END;

;WITH FilteredExam AS
(
    SELECT
        e.Id,
        e.CodiceMinisteriale,
        e.CodiceInterno,
        e.DescrizioneEsame,
        e.BodyPartId
    FROM dbo.Exam e
    WHERE
        @NormalizedSearchText IS NULL
        OR
        (
            @SearchField = N'CodiceMinisteriale'
            AND UPPER(e.CodiceMinisteriale) LIKE N'%' + UPPER(@NormalizedSearchText) + N'%'
        )
        OR
        (
            @SearchField = N'CodiceInterno'
            AND UPPER(e.CodiceInterno) LIKE N'%' + UPPER(@NormalizedSearchText) + N'%'
        )
        OR
        (
            @SearchField = N'DescrizioneEsame'
            AND UPPER(e.DescrizioneEsame) LIKE N'%' + UPPER(@NormalizedSearchText) + N'%'
        )
)

-- 1) Ambulatori visibili nel primo pannello
SELECT DISTINCT
    r.Id,
    r.Name
FROM dbo.Room r
INNER JOIN dbo.ExamRoom er ON er.RoomId = r.Id
INNER JOIN FilteredExam fe ON fe.Id = er.ExamId
ORDER BY r.Name;

-- 2) Parti del corpo visibili nel secondo pannello
SELECT DISTINCT
    bp.Id,
    bp.Name
FROM dbo.BodyPart bp
INNER JOIN FilteredExam fe ON fe.BodyPartId = bp.Id
INNER JOIN dbo.ExamRoom er ON er.ExamId = fe.Id
WHERE
    @SelectedRoomId IS NULL
    OR er.RoomId = @SelectedRoomId
ORDER BY bp.Name;

-- 3) Esami visibili nel terzo pannello
SELECT
    fe.Id,
    fe.CodiceMinisteriale,
    fe.CodiceInterno,
    fe.DescrizioneEsame,
    fe.BodyPartId
FROM FilteredExam fe
INNER JOIN dbo.ExamRoom er ON er.ExamId = fe.Id
WHERE
    (@SelectedRoomId IS NULL OR er.RoomId = @SelectedRoomId)
    AND (@SelectedBodyPartId IS NULL OR fe.BodyPartId = @SelectedBodyPartId)
ORDER BY fe.DescrizioneEsame, fe.CodiceInterno;
