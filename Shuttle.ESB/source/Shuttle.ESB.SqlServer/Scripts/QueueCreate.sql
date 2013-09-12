CREATE TABLE [dbo].[{0}](
	[SequenceId] [int] IDENTITY(1,1) NOT NULL,
	[MessageId] [uniqueidentifier] NOT NULL,
	[MessageBody] [varbinary](max) NOT NULL,
CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
(
	[SequenceId] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
