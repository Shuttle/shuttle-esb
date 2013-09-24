IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[SubscriberMessageType]') AND type = 'U')
	select 1
else
	select 0