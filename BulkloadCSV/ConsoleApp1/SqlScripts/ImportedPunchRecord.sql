CREATE TABLE [dbo].[ImportedPunchRecord] (
    [MachineID]     UNIQUEIDENTIFIER  NOT NULL,
    [CardID]        UNIQUEIDENTIFIER  NOT NULL,
    [PunchIn]       DATETIME NOT NULL,
    [PunchOut]      DATETIME NOT NULL,
    [FileName]      NVARCHAR (100)  NOT NULL,
    [LineNumber]    INT NOT NULL,
    [DateOfImport]  DATE DEFAULT (getdate()) NOT NULL,
);