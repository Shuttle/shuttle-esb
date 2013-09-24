CREATE TABLE [dbo].[SubscriberMessageType](
	[MessageType] [varchar](250) NOT NULL,
	[InboxWorkQueueUri] [varchar](130) NOT NULL,
	[AcceptedBy] varchar(250) NULL,
	[AcceptedDate] [datetime] NULL,
 CONSTRAINT [PK_SubscriberMessageType] PRIMARY KEY CLUSTERED 
	(
		[MessageType] ASC,
		[InboxWorkQueueUri] ASC
	) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
