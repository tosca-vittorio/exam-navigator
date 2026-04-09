SET NOCOUNT ON;
GO

INSERT INTO dbo.BodyPart (Name)
VALUES
    (N'Testa'),
    (N'Arti superiori'),
    (N'Addome'),
    (N'Torace');
GO

INSERT INTO dbo.Room (Name)
VALUES
    (N'Radiologia'),
    (N'Tac1'),
    (N'Tac2'),
    (N'Risonanza'),
    (N'EcografiaPrivitera'),
    (N'EcografiaMassimino'),
    (N'EcografiaDoppler');
GO

INSERT INTO dbo.Exam (CodiceMinisteriale, CodiceInterno, DescrizioneEsame, BodyPartId)
VALUES
    (N'RXMDX001', N'INTRX001', N'RX mano Dx', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Arti superiori')),
    (N'RMNCR001', N'INTRM001', N'RMN cranio', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Testa')),
    (N'ECOADD01', N'INTEC001', N'Eco Addome', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Addome')),
    (N'TACTOR01', N'INTTC001', N'TAC torace', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Torace')),
    (N'ECODOP01', N'INTDP001', N'Eco Doppler TSA', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Testa')),
    (N'RXPOLS01', N'INTRX002', N'RX polso sx', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Arti superiori'));
GO

INSERT INTO dbo.ExamRoom (ExamId, RoomId)
VALUES
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRX001'), (SELECT Id FROM dbo.Room WHERE Name = N'Radiologia')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRM001'), (SELECT Id FROM dbo.Room WHERE Name = N'Risonanza')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTEC001'), (SELECT Id FROM dbo.Room WHERE Name = N'EcografiaPrivitera')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTEC001'), (SELECT Id FROM dbo.Room WHERE Name = N'EcografiaMassimino')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTTC001'), (SELECT Id FROM dbo.Room WHERE Name = N'Tac1')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTTC001'), (SELECT Id FROM dbo.Room WHERE Name = N'Tac2')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTDP001'), (SELECT Id FROM dbo.Room WHERE Name = N'EcografiaDoppler')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRX002'), (SELECT Id FROM dbo.Room WHERE Name = N'Radiologia'));
GO
