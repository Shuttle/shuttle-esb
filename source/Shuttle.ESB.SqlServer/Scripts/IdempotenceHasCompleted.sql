if EXISTS (SELECT * FROM [dbo].[IdempotenceHistory] WHERE MessageId = @MessageId)
	select 1
else
	select 0
