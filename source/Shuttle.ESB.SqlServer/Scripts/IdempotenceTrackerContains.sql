if EXISTS (SELECT * FROM [dbo].[IdempotenceTracker] WHERE MessageId = @MessageId)
	select 1
else
	select 0
