if
	EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[IdempotenceTracker]') AND type = 'U')
	select 1
else
	select 0