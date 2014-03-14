if EXISTS (SELECT * FROM [dbo].[Idempotence] WHERE MessageId = @MessageId)
	select 1
else
	select 0
