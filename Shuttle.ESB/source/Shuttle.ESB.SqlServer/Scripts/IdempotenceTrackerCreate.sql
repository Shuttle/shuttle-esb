CREATE TABLE [dbo].[IdempotenceTracker](
	[MessageId] [uniqueidentifier] NOT NULL,
	[RegisteredDate] [datetime] NOT NULL,
CONSTRAINT [PK_IdempotenceTracker] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[IdempotenceTracker] ADD  CONSTRAINT [DF_IdempotenceTracker_RegisteredDate]  DEFAULT (getdate()) FOR [RegisteredDate]
GO

