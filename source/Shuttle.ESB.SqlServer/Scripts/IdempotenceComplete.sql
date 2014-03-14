insert into [dbo].[IdempotenceHistory]
	(
		MessageId,
		InboxWorkQueueUri,
		DateStarted
	)
select
	MessageId,
	InboxWorkQueueUri,
	DateStarted
from 
	Idempotence
where
	MessageId = @MessageId

delete from [dbo].[Idempotence] where MessageId = @MessageId
