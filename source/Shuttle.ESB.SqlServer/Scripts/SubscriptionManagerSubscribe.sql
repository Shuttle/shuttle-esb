if not exists (select null from [dbo].[SubscriberMessageType] where InboxWorkQueueUri = @InboxWorkQueueUri and MessageType = @MessageType)
	insert into [dbo].[SubscriberMessageType]
		(InboxWorkQueueUri, MessageType, AcceptedBy, AcceptedDate) 
	values 
		(@InboxWorkQueueUri, @MessageType, 'System', getdate())
