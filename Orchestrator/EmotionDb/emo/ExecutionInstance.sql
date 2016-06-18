﻿CREATE TABLE [emo].[ExecutionInstance]
(
	[Id] INT IDENTITY PRIMARY KEY,
	[StartTime] DATETIME DEFAULT GETUTCNOW(),
	[EndTime] DATETIME DEFAULT NULL,
	[FileName] NVARCHAR(MAX) NOT NULL,
	[Width] INT NOT NULL,
	[Height] INT NOT NULL,
)