IF OBJECT_ID(N'dbo.ExamRoom', N'U') IS NOT NULL DROP TABLE dbo.ExamRoom;
IF OBJECT_ID(N'dbo.Exam', N'U') IS NOT NULL DROP TABLE dbo.Exam;
IF OBJECT_ID(N'dbo.Room', N'U') IS NOT NULL DROP TABLE dbo.Room;
IF OBJECT_ID(N'dbo.BodyPart', N'U') IS NOT NULL DROP TABLE dbo.BodyPart;
GO

CREATE TABLE dbo.BodyPart
(
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_BodyPart PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_BodyPart_Name UNIQUE (Name)
);
GO

CREATE TABLE dbo.Room
(
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_Room PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Room_Name UNIQUE (Name)
);
GO

CREATE TABLE dbo.Exam
(
    Id INT IDENTITY(1,1) NOT NULL,
    CodiceMinisteriale NVARCHAR(10) NOT NULL,
    CodiceInterno NVARCHAR(10) NOT NULL,
    DescrizioneEsame NVARCHAR(100) NOT NULL,
    BodyPartId INT NOT NULL,
    CONSTRAINT PK_Exam PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Exam_BodyPart FOREIGN KEY (BodyPartId) REFERENCES dbo.BodyPart(Id),
    CONSTRAINT UQ_Exam_CodiceMinisteriale UNIQUE (CodiceMinisteriale),
    CONSTRAINT UQ_Exam_CodiceInterno UNIQUE (CodiceInterno)
);
GO

CREATE TABLE dbo.ExamRoom
(
    ExamId INT NOT NULL,
    RoomId INT NOT NULL,
    CONSTRAINT PK_ExamRoom PRIMARY KEY CLUSTERED (ExamId, RoomId),
    CONSTRAINT FK_ExamRoom_Exam FOREIGN KEY (ExamId) REFERENCES dbo.Exam(Id),
    CONSTRAINT FK_ExamRoom_Room FOREIGN KEY (RoomId) REFERENCES dbo.Room(Id)
);
GO
