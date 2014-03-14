CREATE TABLE [dbo].[IdempotenceHistory](
	[MessageId] [uniqueidentifier] NOT NULL,
	[InboxWorkQueueUri] [varchar](265) NOT NULL,
	[DateStarted] [datetime] NOT NULL,
	[DateCompleted] [datetime] NOT NULL,
 CONSTRAINT [PK_IdempotenceHistory] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[IdempotenceDeferredMessage](
	[MessageId] [uniqueidentifier] NOT NULL,
	[MessageBody] [image] NOT NULL,
 CONSTRAINT [PK_IdempotenceDeferredMessage] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Idempotence](
	[MessageId] [uniqueidentifier] NOT NULL,
	[InboxWorkQueueUri] [varchar](265) NOT NULL,
	[DateStarted] [datetime] NOT NULL,
 CONSTRAINT [PK_Idempotence] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdempotenceHistory] ADD  CONSTRAINT [DF_IdempotenceHistory_DateStarted]  DEFAULT (getdate()) FOR [DateCompleted]
GO
ALTER TABLE [dbo].[Idempotence] ADD  CONSTRAINT [DF_Idempotence_DateStarted]  DEFAULT (getdate()) FOR [DateStarted]
GO
