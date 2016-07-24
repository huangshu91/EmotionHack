CREATE TABLE [emo].[EmotionScore]
(
	[Id] INT IDENTITY PRIMARY KEY,
	[ExecutionId] INT NOT NULL,
	[Anger] FLOAT NOT NULL,
	[Contempt] FLOAT NOT NULL,
	[Disgust] FLOAT NOT NULL,
	[Fear] FLOAT NOT NULL,
	[Happiness] FLOAT NOT NULL,
	[Neutral] FLOAT NOT NULL,
	[Sadness] FLOAT NOT NULL,
	[Surprise] FLOAT NOT NULL,

	[TimeStamp] FLOAT NOT NULL,
	[StartTime] DATETIME NOT NULL,
	[EndTime] DATETIME NOT NULL,

	CONSTRAINT [FK_EmotionScore_ExecutionId] FOREIGN KEY([ExecutionId]) REFERENCES [emo].[ExecutionInstance]([Id])
)
