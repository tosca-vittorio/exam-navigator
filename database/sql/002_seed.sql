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
    (N'EcografiaDoppler'),
    (N'Radiologia2'),
    (N'Tac3'),
    (N'Risonanza2'),
    (N'EcografiaCentrale');
GO

INSERT INTO dbo.Exam (CodiceMinisteriale, CodiceInterno, DescrizioneEsame, BodyPartId)
VALUES
    (N'RXMDX001', N'INTRX001', N'RX mano Dx', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Arti superiori')),
    (N'RMNCR001', N'INTRM001', N'RMN cranio', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Testa')),
    (N'ECOADD01', N'INTEC001', N'Eco Addome', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Addome')),
    (N'TACTOR01', N'INTTC001', N'TAC torace', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Torace')),
    (N'ECODOP01', N'INTDP001', N'Eco Doppler TSA', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Testa')),
    (N'RXPOLS01', N'INTRX002', N'RX polso Sx', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Arti superiori')),

    (N'RXSPDX01', N'INTRX003', N'RX spalla Dx', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Arti superiori')),
    (N'RXGOMSX1', N'INTRX004', N'RX gomito Sx', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Arti superiori')),
    (N'TACENC01', N'INTTC002', N'TAC encefalo', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Testa')),
    (N'RMNSCL01', N'INTRM002', N'RMN sella turcica', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Testa')),
    (N'ECOADC01', N'INTEC002', N'Eco addome completo', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Addome')),
    (N'TACADD01', N'INTTC003', N'TAC addome', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Addome')),
    (N'RXTORPA1', N'INTRX005', N'RX torace PA', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Torace')),
    (N'ECOCOL01', N'INTDP002', N'Eco collo', (SELECT Id FROM dbo.BodyPart WHERE Name = N'Testa'));
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
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRX002'), (SELECT Id FROM dbo.Room WHERE Name = N'Radiologia')),

    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRX003'), (SELECT Id FROM dbo.Room WHERE Name = N'Radiologia')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRX004'), (SELECT Id FROM dbo.Room WHERE Name = N'Radiologia')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTTC002'), (SELECT Id FROM dbo.Room WHERE Name = N'Tac1')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTTC002'), (SELECT Id FROM dbo.Room WHERE Name = N'Tac2')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRM002'), (SELECT Id FROM dbo.Room WHERE Name = N'Risonanza')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTEC002'), (SELECT Id FROM dbo.Room WHERE Name = N'EcografiaPrivitera')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTEC002'), (SELECT Id FROM dbo.Room WHERE Name = N'EcografiaMassimino')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTTC003'), (SELECT Id FROM dbo.Room WHERE Name = N'Tac1')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTTC003'), (SELECT Id FROM dbo.Room WHERE Name = N'Tac2')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRX005'), (SELECT Id FROM dbo.Room WHERE Name = N'Radiologia')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTDP002'), (SELECT Id FROM dbo.Room WHERE Name = N'EcografiaDoppler')),

    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRX002'), (SELECT Id FROM dbo.Room WHERE Name = N'Radiologia2')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRX003'), (SELECT Id FROM dbo.Room WHERE Name = N'Radiologia2')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRX005'), (SELECT Id FROM dbo.Room WHERE Name = N'Radiologia2')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTTC001'), (SELECT Id FROM dbo.Room WHERE Name = N'Tac3')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTTC002'), (SELECT Id FROM dbo.Room WHERE Name = N'Tac3')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTTC003'), (SELECT Id FROM dbo.Room WHERE Name = N'Tac3')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRM001'), (SELECT Id FROM dbo.Room WHERE Name = N'Risonanza2')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTRM002'), (SELECT Id FROM dbo.Room WHERE Name = N'Risonanza2')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTEC001'), (SELECT Id FROM dbo.Room WHERE Name = N'EcografiaCentrale')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTEC002'), (SELECT Id FROM dbo.Room WHERE Name = N'EcografiaCentrale')),
    ((SELECT Id FROM dbo.Exam WHERE CodiceInterno = N'INTDP002'), (SELECT Id FROM dbo.Room WHERE Name = N'EcografiaCentrale'));
GO
