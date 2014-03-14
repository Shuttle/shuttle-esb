delete 
from 
	idm
from
	[dbo].[IdempotenceDeferredMessage] idm
inner join
	[dbo].[Idempotence] i on
	(
		idm.MessageId = i.MessageId
		and
		i.InboxWorkQueueUri = @InboxWorkQueueUri
	)

delete from [dbo].[Idempotence] where InboxWorkQueueUri = @InboxWorkQueueUri
